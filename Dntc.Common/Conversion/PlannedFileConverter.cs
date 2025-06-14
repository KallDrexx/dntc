using Dntc.Common.Conversion.Mutators;
using Dntc.Common.Definitions;
using Dntc.Common.Definitions.ReferenceTypeSupport;
using Dntc.Common.OpCodeHandling;
using Dntc.Common.Planning;
using Dntc.Common.Syntax;
using Dntc.Common.Syntax.Expressions;
using Dntc.Common.Syntax.Statements;
using Dntc.Common.Syntax.Statements.Generators;
using Mono.Cecil.Cil;

namespace Dntc.Common.Conversion;

public class PlannedFileConverter
{
    private readonly ConversionCatalog _conversionCatalog;
    private readonly DefinitionCatalog _definitionCatalog;
    private readonly List<IStatementGenerator> _statementGenerators = [];
    private readonly bool _debugLogging;
    private readonly IMemoryManagementActions _memoryManagement;

    public PlannedFileConverter(
        ConversionCatalog conversionCatalog, 
        DefinitionCatalog definitionCatalog, 
        bool debugLogging,
        IMemoryManagementActions memoryManagement)
    {
        _conversionCatalog = conversionCatalog;
        _definitionCatalog = definitionCatalog;
        _debugLogging = debugLogging;
        _memoryManagement = memoryManagement;
    }

    public HeaderFile Convert(PlannedHeaderFile plannedHeaderFile)
    {
        var guard = new HeaderGuard(plannedHeaderFile.Name);
        var includes = plannedHeaderFile.ReferencedHeaders
            .Select(x => new IncludeClause(x))
            .ToArray();

        var typeDeclarations = plannedHeaderFile.DeclaredTypes
            .Select(x => new { ConversionInfo = x, Definition = _definitionCatalog.Get(x.IlName) })
            .Select(x => new TypeDeclaration(x.ConversionInfo, x.Definition!, _conversionCatalog))
            .ToArray();

        var methodDeclarations = plannedHeaderFile.DeclaredMethods
            .Select(x => new { ConversionInfo = x, Definition = _definitionCatalog.Get(x.MethodId) })
            .Select(x => new MethodDeclaration(x.ConversionInfo, x.Definition!, _conversionCatalog))
            .ToArray();

        var globals = plannedHeaderFile.DeclaredGlobals
            .Select(x => new { GlobalInfo = x, TypeInfo = x.FieldTypeConversionInfo })
            .Select(x => new FieldDeclaration(x.GlobalInfo, FieldDeclaration.FieldFlags.IsHeaderDeclaration))
            .ToArray();

        return new HeaderFile(guard, includes, typeDeclarations, methodDeclarations, globals);
    }

    public SourceFile Convert(PlannedSourceFile plannedSourceFile)
    {
        var includes = plannedSourceFile.ReferencedHeaders
            .Select(x => new IncludeClause(x))
            .ToArray();

        var methodBlocks = plannedSourceFile.ImplementedMethods
            .Select(x =>
            {
                var definition = _definitionCatalog.Get(x.MethodId);
                var declaration = new MethodDeclaration(x, definition!, _conversionCatalog);
                var statements = GetMethodStatements(definition!, x);
                
                return new MethodBlock(x, statements, declaration);
            })
            .Where(x => x.Statements.Count > 0)
            .ToArray();

        var globals = plannedSourceFile.ImplementedGlobals
            .Select(x => new { GlobalInfo = x, TypeInfo = x.FieldTypeConversionInfo })
            .Select(x => new FieldDeclaration(x.GlobalInfo, FieldDeclaration.FieldFlags.None))
            .ToArray();

        var typeDeclarations = plannedSourceFile.DeclaredTypes
            .Select(x => new { ConversionInfo = x, Definition = _definitionCatalog.Get(x.IlName) })
            .Select(x => new TypeDeclaration(x.ConversionInfo, x.Definition!, _conversionCatalog))
            .ToArray();

        var methodDeclarations = plannedSourceFile.DeclaredMethods
            .Select(x => new { ConversionInfo = x, Definition = _definitionCatalog.Get(x.MethodId) })
            .Select(x => new MethodDeclaration(x.ConversionInfo, x.Definition!, _conversionCatalog))
            .ToArray();

        return new SourceFile(includes, methodBlocks, globals, typeDeclarations, methodDeclarations);
    }

    private IReadOnlyList<CStatementSet> GetMethodStatements(
        DefinedMethod definition, 
        MethodConversionInfo conversionInfo)
    {
        return definition switch
        {
            DotNetDefinedMethod dotNetDefinedMethod => GetMethodStatements(dotNetDefinedMethod, conversionInfo),
            CustomDefinedMethod customDefinedMethod => GetMethodStatements(customDefinedMethod),
            NativeDefinedMethod => [],
            _ => throw new NotSupportedException(definition.GetType().FullName)
        };
    }

    private IReadOnlyList<CStatementSet> GetMethodStatements(
        DotNetDefinedMethod dotNetDefinedMethod, 
        MethodConversionInfo conversionInfo)
    {
        if (_debugLogging)
        {
            Console.WriteLine($"{dotNetDefinedMethod.Definition.FullName}:");
        }
        
        var statements = new List<CStatementSet>();
        var methodInstruction = dotNetDefinedMethod.Definition.Body.Instructions.First();
        OnBeforeGenerateInstruction(statements, dotNetDefinedMethod, methodInstruction);

        // If this isn't a void return type, define a local variable that will hold the return value.
        // This is needed so we can compute a return value prior to the function untracking any
        // used reference types.
        if (dotNetDefinedMethod.ReturnType != new IlTypeName(typeof(void).FullName!))
        {
            var returnType = _conversionCatalog.Find(dotNetDefinedMethod.ReturnType);
            statements.Add(
                new LocalDeclarationStatementSet(
                    new Variable(returnType, Utils.ReturnVariableName(), returnType.IsPointer ? 1 : 0)));
        }
        
        // Add local statements
        HashSet<string> locals = [];
        for (var x = 0; x < dotNetDefinedMethod.Definition.Body.Variables.Count; x++)
        {
            var local = dotNetDefinedMethod.Locals[x];
            var localType = _conversionCatalog.Find(local.Type);
            var name = Utils.LocalName(dotNetDefinedMethod.Definition, x);
            var variable = new Variable(localType, name, local.Type.IsPointer() ? 1 : 0);

            if (locals.Add(name))
            {
                statements.Add(new LocalDeclarationStatementSet(variable));
            }
            else
            {
                // Error if the Variable is of a different type?
                // but not possible? otherwise IL would not be valid?
            }
        }

        // We need to add GC track calls for each reference type argument, as `starg` op codes may cause
        // them to be untracked, and if we don't do an initial increment than callers may have the values
        // freed out from under them.
        foreach (var parameter in dotNetDefinedMethod.Parameters)
        {
            var paramType = _conversionCatalog.Find(parameter.Type);
            if (paramType.IsReferenceType && parameter.Name != Utils.ThisArgumentName && parameter.IsReference)
            {
                // Skip GC tracking for ref reference type parameters - they represent addresses 
                // of caller variables, not references that need tracking
                if (parameter.IsReferenceTypeByRef && paramType.IsReferenceType)
                {
                    continue;
                }
                
                var variable = new VariableValueExpression(
                    new Variable(paramType, parameter.Name, 1));

                statements.Add(new GcTrackFunctionCallStatement(variable, _conversionCatalog));
            }
        }

        OnAfterGenerateInstruction(statements, dotNetDefinedMethod, methodInstruction);

        var startingOffset = (int?) null;
        var expressionStack = new ExpressionStack();
        var checkpoints = new Dictionary<int, Variable>();

        foreach (var instruction in dotNetDefinedMethod.Definition.Body.Instructions)
        {
            startingOffset ??= instruction.Offset;

            if (checkpoints.TryGetValue(instruction.Offset, out var checkpointVariable))
            {
                // We hit a checkpoint target, so we need to save the current stack items to
                // the checkpoint variable, then load the checkpoint variable to the expression stack
                var checkpointAssign = SaveCheckpoint(expressionStack, checkpoints, instruction.Offset);
                checkpointAssign.StartingIlOffset = instruction.Offset - 1;
                checkpointAssign.LastIlOffset = instruction.Offset - 1;
                statements.Add(checkpointAssign);
                
                expressionStack.Push(new VariableValueExpression(checkpointVariable));
                
                // Make sure the syntax tree has a local declaration for this checkpoint variable
                var localDeclaration = new LocalDeclarationStatementSet(checkpointVariable);
                statements.Insert(0, localDeclaration);
            }

            if (!KnownOpCodeHandlers.OpCodeHandlers.TryGetValue(instruction.OpCode.Code, out var handler))
            {
                var message = $"No known handler for op code {instruction.OpCode.Code}";
                throw new InvalidOperationException(message);
            }

            var referenceTypeLocalVariables = statements
                .Flatten()
                .OfType<LocalDeclarationStatementSet>()
                .Select(x => x.Variable)
                .Where(x => x.Type.IsReferenceType)
                .Where(x => x.Name != Utils.ReturnVariableName())
                .ToArray();

            var startStackSize = expressionStack.Count;
            OpCodeHandlingResult result;
            try
            {
                result = handler.Handle(new HandleContext(
                    instruction,
                    expressionStack,
                    conversionInfo,
                    dotNetDefinedMethod,
                    _conversionCatalog,
                    _definitionCatalog,
                    _memoryManagement,
                    referenceTypeLocalVariables));
            }
            catch (Exception exception)
            {
                var debugInfo = CecilUtils.LoggedSequencePointInfo(
                    CecilUtils.GetSequencePoint(
                        dotNetDefinedMethod.Definition,
                        instruction));

                var message = $"Exception occurred transpiling method '{dotNetDefinedMethod.Id}' on instruction " +
                              $"IL_{instruction.Offset:x4}: {instruction.OpCode.Code} ({instruction.Operand}) {debugInfo}";
                throw new Exception(message, exception);
            }

            if (result.CheckpointUntilTargetOffset != null)
            {
                var statement = SaveCheckpoint(expressionStack, checkpoints, result.CheckpointUntilTargetOffset.Value);
                
                // Set it as one instruction before the instruction that caused the checkpointing, to make sure
                // any gotos go past the checkpoint.
                statement.StartingIlOffset = instruction.Offset - 1;
                statement.LastIlOffset = instruction.Offset - 1;

                OnBeforeGenerateInstruction(statements, dotNetDefinedMethod, instruction);
                statements.Add(statement);
                OnAfterGenerateInstruction(statements, dotNetDefinedMethod, instruction);
            }

            if (result.StatementSet != null)
            {
                result.StatementSet.StartingIlOffset = startingOffset.Value;
                result.StatementSet.LastIlOffset = instruction.Offset;
                
                OnBeforeGenerateInstruction(statements, dotNetDefinedMethod, instruction);
                statements.Add(result.StatementSet);
                OnAfterGenerateInstruction(statements, dotNetDefinedMethod, instruction);
                startingOffset = null;
            }

            if (_debugLogging)
            {
                Console.WriteLine($"\tIL_{instruction.Offset:x4}: {instruction.OpCode.Code} {instruction.Operand} " +
                                  $"(stack: {startStackSize}->{expressionStack.Count})");
            }
        }

        if (_debugLogging)
        {
            Console.WriteLine();
        }

        return statements;
    }

    private void OnBeforeGenerateInstruction(List<CStatementSet> statements, DotNetDefinedMethod dotNetDefinedMethod,
        Instruction methodInstruction)
    {
        foreach (var statementGenerator in _statementGenerators)
        {
            statements.AddRange(statementGenerator.Before(statements, dotNetDefinedMethod.Definition, methodInstruction));
        }
    }
    
    private void OnAfterGenerateInstruction(List<CStatementSet> statements, DotNetDefinedMethod dotNetDefinedMethod,
        Instruction methodInstruction)
    {
        foreach (var instructionGenerator in _statementGenerators)
        {
            statements.AddRange(instructionGenerator.After(statements, dotNetDefinedMethod.Definition, methodInstruction));
        }
    }

    private static CStatementSet SaveCheckpoint(ExpressionStack stack, Dictionary<int, Variable> checkpoints, int targetOffset)
    {
        // Right now assuming we only need one checkpoint var per target IL, unsure if that's always true.
        if (stack.Count != 1)
        {
            var message = $"Expected a checkpoint with only one value in the expression stack, but the stack " +
                          $"had a count of {stack.Count}";
            throw new InvalidOperationException(message);
        }

        var item = stack.Pop(1)[0];
        if (!checkpoints.TryGetValue(targetOffset, out var variable))
        {
            variable = new Variable(item.ResultingType, $"__checkpoint_for_il{targetOffset:x4}", item.PointerDepth);
            checkpoints.Add(targetOffset, variable);
        }

        var variableExpression = new VariableValueExpression(variable);
        return new AssignmentStatementSet(variableExpression, item);
    }

    private IReadOnlyList<CStatementSet> GetMethodStatements(CustomDefinedMethod customDefinedMethod)
    {
        var statementSet = customDefinedMethod.GetCustomImplementation(_conversionCatalog);
        return statementSet != null
            ? [statementSet]
            : [];
    }

    public void AddInstructionGenerator(IStatementGenerator plugin)
    {
        _statementGenerators.Add(plugin);
    }
}