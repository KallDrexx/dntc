﻿using Dntc.Common.Definitions;
using Dntc.Common.OpCodeHandling;
using Dntc.Common.Planning;
using Dntc.Common.Syntax;
using Dntc.Common.Syntax.Statements;

namespace Dntc.Common.Conversion;

public class PlannedFileConverter
{
    private readonly ConversionCatalog _conversionCatalog;
    private readonly DefinitionCatalog _definitionCatalog;
    private readonly bool _debugLogging; 

    public PlannedFileConverter(
        ConversionCatalog conversionCatalog, 
        DefinitionCatalog definitionCatalog, 
        bool debugLogging)
    {
        _conversionCatalog = conversionCatalog;
        _definitionCatalog = definitionCatalog;
        _debugLogging = debugLogging;
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

        return new HeaderFile(guard, includes, typeDeclarations, methodDeclarations);
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
            .ToArray();

        return new SourceFile(includes, methodBlocks);
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
        
        // Add local statements
        for (var x = 0; x < dotNetDefinedMethod.Locals.Count; x++)
        {
            var local = dotNetDefinedMethod.Locals[x];
            var localType = _conversionCatalog.Find(local.Type);
            var variable = new Variable(localType, Utils.LocalName(x), local.IsReference);
            statements.Add(new LocalDeclarationStatementSet(variable));
        }
        
        var startingOffset = (int?) null;
        var expressionStack = new ExpressionStack();
        foreach (var instruction in dotNetDefinedMethod.Definition.Body.Instructions)
        {
            startingOffset ??= instruction.Offset;

            if (!KnownOpCodeHandlers.OpCodeHandlers.TryGetValue(instruction.OpCode.Code, out var handler))
            {
                var message = $"No known handler for op code {instruction.OpCode.Code}";
                throw new InvalidOperationException(message);
            }

            var startStackSize = expressionStack.Count;
            var result = handler.Handle(
                instruction,
                expressionStack,
                conversionInfo,
                _conversionCatalog);

            if (result.StatementSet != null)
            {
                result.StatementSet.StartingIlOffset = startingOffset.Value;
                result.StatementSet.LastIlOffset = instruction.Offset;
                statements.Add(result.StatementSet);

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

    private IReadOnlyList<CStatementSet> GetMethodStatements(CustomDefinedMethod customDefinedMethod)
    {
        var statementSet = customDefinedMethod.GetCustomImplementation();
        return statementSet != null
            ? [statementSet]
            : [];
    }
}