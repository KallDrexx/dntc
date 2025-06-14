using Dntc.Common.Conversion;
using Dntc.Common.Definitions;
using Dntc.Common.Syntax.Expressions;
using Dntc.Common.Syntax.Statements;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Dntc.Common.OpCodeHandling.Handlers;

public class StoreHandlers : IOpCodeHandlerCollection
{
    public IReadOnlyDictionary<Code, IOpCodeHandler> Handlers { get; } = new Dictionary<Code, IOpCodeHandler>
    {
        { Code.Stfld, new StFldHandler() },
        { Code.Stsfld, new StFldHandler() },
        { Code.Stobj, new StObjHandler() },
        { Code.Starg, new StArgHandler() },
        { Code.Starg_S, new StArgHandler() },

        { Code.Stind_I, new StIndHandler() },
        { Code.Stind_I1, new StIndHandler() },
        { Code.Stind_I2, new StIndHandler() },
        { Code.Stind_I4, new StIndHandler() },
        { Code.Stind_I8, new StIndHandler() },
        { Code.Stind_R4, new StIndHandler() },
        { Code.Stind_R8, new StIndHandler() },
        { Code.Stind_Ref, new StIndRefHandler() },

        { Code.Stloc, new StLocHandler(null) },
        { Code.Stloc_0, new StLocHandler(0) },
        { Code.Stloc_1, new StLocHandler(1) },
        { Code.Stloc_2, new StLocHandler(2) },
        { Code.Stloc_3, new StLocHandler(3) },
        { Code.Stloc_S, new StLocHandler(null) },
    };

    /// <summary>
    /// Searches if we are about to store to a variable whose value is already referenced in the
    /// expression stack. If that's the case, we need to save the value to a temporary variable, and
    /// replace the current variable with the temporary one for proper behavior.
    /// </summary>
    /// <returns>Statement sets for temp variable replacement only if one is required</returns>
    private static CStatementSet? HandleReferencedVariable(
        ExpressionStack expressionStack,
        VariableValueExpression variable,
        int currentIlOffset)
    {
        var name = $"__temp_{currentIlOffset:x4}";
        var tempVariable = variable.Variable with { Name = name };
        var tempVariableExpression = new VariableValueExpression(tempVariable);

        if (expressionStack.ReplaceExpression(variable, tempVariableExpression))
        {
            // At least one expression in the stack was replaced
            // TODO: Do I need to do an untrack here for reference types?
            var localDeclaration = new LocalDeclarationStatementSet(tempVariable);
            var assignment = new AssignmentStatementSet(tempVariableExpression, variable);
            return new CompoundStatementSet([localDeclaration, assignment]);
        }

        return null;
    }

    private class StFldHandler : IOpCodeHandler
    {
        public OpCodeHandlingResult Handle(HandleContext context)
        {
            var reference = (FieldReference)context.CurrentInstruction.Operand;
            var field = reference.Resolve();
            var fieldConversionInfo = context.ConversionCatalog.Find(new IlFieldId(field.FullName));

            var statements = new List<CStatementSet>();
            if (field.IsStatic)
            {
                var items = context.ExpressionStack.Pop(1);
                var value = items[0];

                var variable = new Variable(
                    fieldConversionInfo.FieldTypeConversionInfo,
                    fieldConversionInfo.NameInC.Value, 
                    0);

                var left = new VariableValueExpression(variable);
                var right = value;
                var assignment = new AssignmentStatementSet(left, right);

                // If the static field is referenced elsewhere in the stack, we need to replace
                // those expressions with a locally stored value of the static field before the
                // assignment takes place. Otherwise, code coming up that uses the value in the expression
                // stack will see the wrong value.
                var referenceReplacements = HandleReferencedVariable(
                    context.ExpressionStack,
                    left,
                    context.CurrentInstruction.Offset);

                if (referenceReplacements != null)
                {
                    statements.Add(referenceReplacements);
                }

                statements.Add(assignment);
            }
            else
            {
                var items = context.ExpressionStack.Pop(2);
                var value = items[0];
                var obj = items[1];

                var left = GetFieldAccessExpression(field, fieldConversionInfo, obj, context);
                var right = value;

                if (left.ResultingType.IsReferenceType)
                {
                    statements.Add(new GcUntrackFunctionCallStatement(left, context.ConversionCatalog));
                }

                // I think this might need to handle referenced variable replacement but using field access expression
                // instead of variable value expressions. I can't get a good use case to reproduce it though, so
                // I'm going to leave it as is for now pending a good test case. In most test cases I've tried, the
                // field loading does not tend to use the dup trick that static fields use to keep a hanging reference
                // on the evaluation stack.
                statements.Add(new AssignmentStatementSet(left, right));

                if (left.ResultingType.IsReferenceType)
                {
                    statements.Add(new GcTrackFunctionCallStatement(left, context.ConversionCatalog));
                }
            }

            return new OpCodeHandlingResult(new CompoundStatementSet(statements));
        }

        public OpCodeAnalysisResult Analyze(AnalyzeContext context)
        {
            var reference = (FieldReference)context.CurrentInstruction.Operand;
            var field = reference.Resolve();

            // We only need to return the declaring type if the field isn't static. If the
            // field is static than we don't actually need to reference the declaring type
            // in code.
            var declaringTypes = field.IsStatic
                ? Array.Empty<IlTypeName>()
                : [new IlTypeName(field.DeclaringType.FullName)];

            return new OpCodeAnalysisResult
            {
                ReferencedTypes = new HashSet<IlTypeName>(declaringTypes),
                ReferencedGlobal = field.IsStatic ? field : null,
            };
        }

        private static CBaseExpression GetFieldAccessExpression(
            FieldDefinition field,
            FieldConversionInfo fieldConversionInfo,
            CBaseExpression objectExpression,
            HandleContext context)
        {
            var originalObjectExpression = objectExpression;
            while (true)
            {
                var objectInfo = objectExpression.ResultingType;
                var containsField = objectInfo.OriginalTypeDefinition
                    .InstanceFields
                    .Any(x => x.Id.Value == field.FullName);

                if (containsField)
                {
                    var fieldVariable = new Variable(
                        fieldConversionInfo.FieldTypeConversionInfo,
                        fieldConversionInfo.NameInC.Value,
                        field.FieldType.IsByReference ? 1 : 0);

                    // If the object expression is a double pointer (ref reference type), 
                    // we need to dereference it once to access fields
                    var adjustedObjectExpression = objectExpression.PointerDepth == 2 && objectExpression.ResultingType.IsReferenceType
                        ? new AdjustPointerDepthExpression(objectExpression, 1)
                        : objectExpression;

                    return new FieldAccessExpression(adjustedObjectExpression, fieldVariable);
                }

                // If this is a dotnet expression, then check the type's base class
                if (objectInfo.OriginalTypeDefinition is DotNetDefinedType dotNetType
                    && dotNetType.Definition.BaseType != null)
                {
                    // Repeat the process for the base class
                    var parent = context.ConversionCatalog.Find(
                        new IlTypeName(dotNetType.Definition.BaseType.FullName));

                    var fieldVariable = new Variable(parent, "base", 0);
                    
                    // If the object expression is a double pointer (ref reference type), 
                    // we need to dereference it once to access fields
                    var adjustedObjectExpression = objectExpression.PointerDepth == 2 && objectExpression.ResultingType.IsReferenceType
                        ? new AdjustPointerDepthExpression(objectExpression, 1)
                        : objectExpression;
                    
                    objectExpression = new FieldAccessExpression(adjustedObjectExpression, fieldVariable);
                    continue;
                }

                // No matching field found
                var message = $"The type {originalObjectExpression.ResultingType.IlName} or its base types do not have " +
                              $"the field {field.FullName}";
                throw new InvalidOperationException(message);
            }
        }
    }

    private class StIndHandler : IOpCodeHandler
    {
        public OpCodeHandlingResult Handle(HandleContext context)
        {
            var items = context.ExpressionStack.Pop(2);
            var value = items[0];
            var address = items[1];

            var left = new AdjustPointerDepthExpression(address, 0);
            var statement = new AssignmentStatementSet(left, value);

            return new OpCodeHandlingResult(statement);
        }

        public OpCodeAnalysisResult Analyze(AnalyzeContext context)
        {
            return new OpCodeAnalysisResult();
        }
    }

    private class StObjHandler : IOpCodeHandler
    {
        public OpCodeHandlingResult Handle(HandleContext context)
        {
            var items = context.ExpressionStack.Pop(2);
            var objectValue = items[0];
            var address = items[1];

            var left = new AdjustPointerDepthExpression(address, 0);
            var statement = new AssignmentStatementSet(left, objectValue);

            return new OpCodeHandlingResult(statement);
        }

        public OpCodeAnalysisResult Analyze(AnalyzeContext context)
        {
            return new OpCodeAnalysisResult();
        }
    }

    private class StArgHandler : IOpCodeHandler
    {
        public OpCodeHandlingResult Handle(HandleContext context)
        {
            var items = context.ExpressionStack.Pop(1);
            var value = items[0];

            var argIndex = context.CurrentInstruction.Operand switch
            {
                int intOperand => intOperand,
                ParameterDefinition parameterDefinition => parameterDefinition.Index,
                _ => throw new NotSupportedException(context.CurrentInstruction.Operand.GetType().FullName),
            };

            if (context.CurrentMethodConversion.Parameters.Count < argIndex)
            {
                var message = $"starg on argument index {argIndex} is beyond the argument " +
                              $"count of {context.CurrentMethodConversion.Parameters.Count}";
                throw new InvalidOperationException(message);
            }

            var argument = context.CurrentMethodConversion.Parameters[argIndex];
            var argumentInfo = context.ConversionCatalog.Find(argument.TypeName);
            
            var pointerDepth = argument.IsReference ? 1 : 0;
            if (argument.IsReferenceTypeByRef && argumentInfo.IsReferenceType) 
            {
                pointerDepth += 1; // ref reference types get double pointer
            }
            
            var storedVariableExpression = new VariableValueExpression(
                new Variable(argumentInfo, argument.Name, pointerDepth));

            var tempVariable = HandleReferencedVariable(
                context.ExpressionStack,
                storedVariableExpression,
                context.CurrentInstruction.Offset);

            var statements = new List<CStatementSet>();
            if (storedVariableExpression.ResultingType.IsReferenceType)
            {
                statements.Add(new GcUntrackFunctionCallStatement(storedVariableExpression, context.ConversionCatalog));
            }

            if (tempVariable != null)
            {
                statements.Add(tempVariable);
            }

            statements.Add(new AssignmentStatementSet(storedVariableExpression, value));

            if (storedVariableExpression.ResultingType.IsReferenceType)
            {
                statements.Add(new GcTrackFunctionCallStatement(storedVariableExpression, context.ConversionCatalog));
            }

            return new OpCodeHandlingResult(new CompoundStatementSet(statements));
        }

        public OpCodeAnalysisResult Analyze(AnalyzeContext context)
        {
            return new OpCodeAnalysisResult();
        }
    }

    private class StLocHandler(int? index) : IOpCodeHandler
    {
        public OpCodeHandlingResult Handle(HandleContext context)
        {
            var items = context.ExpressionStack.Pop(1);

            int localIndex;
            if (index != null)
            {
                localIndex = index.Value;
            }
            else
            {
                localIndex = context.CurrentInstruction.Operand switch
                {
                    int intIndex => intIndex,
                    VariableDefinition variableDefinition => variableDefinition.Index,
                    _ => throw new NotSupportedException(context.CurrentInstruction.Operand.GetType().FullName),
                };
            }

            if (context.CurrentMethodConversion.Locals.Count <= localIndex)
            {
                var message = $"stloc for local index {localIndex} but method only has " +
                              $"{context.CurrentMethodConversion.Locals.Count} locals";
                throw new InvalidOperationException(message);
            }

            var local = context.CurrentMethodConversion.Locals[localIndex];
            var localInfo = context.ConversionCatalog.Find(local.TypeName);
            var localVariable = new VariableValueExpression(
                new Variable(
                    localInfo,
                    Utils.LocalName(context.CurrentDotNetMethod.Definition, localIndex),
                    local.IsReference ? 1 : 0));

            // For reference types, both the local variable and the value should be pointers,
            // so no pointer depth adjustment is needed
            CBaseExpression right;
            if (localInfo.IsReferenceType && items[0].ResultingType.IsReferenceType)
            {
                right = new AdjustPointerDepthExpression(items[0], 1);
            }
            else
            {
                right = new AdjustPointerDepthExpression(items[0], localVariable.PointerDepth);
            }

            var tempStatement = HandleReferencedVariable(
                context.ExpressionStack,
                localVariable,
                context.CurrentInstruction.Offset);

            var statements = new List<CStatementSet>();
            if (localVariable.ResultingType.IsReferenceType)
            {
                // Since the local is a reference type, we need to untrack the old one
                statements.Add(new GcUntrackFunctionCallStatement(localVariable, context.ConversionCatalog));
            }

            if (tempStatement != null)
            {
                statements.Add(tempStatement);
            }

            statements.Add(new AssignmentStatementSet(localVariable, right));

            if (localVariable.ResultingType.IsReferenceType)
            {
                statements.Add(new GcTrackFunctionCallStatement(localVariable, context.ConversionCatalog));
            }

            return new OpCodeHandlingResult(new CompoundStatementSet(statements));
        }

        public OpCodeAnalysisResult Analyze(AnalyzeContext context)
        {
            return new OpCodeAnalysisResult();
        }
    }

    private class StIndRefHandler : IOpCodeHandler
    {
        public OpCodeHandlingResult Handle(HandleContext context)
        {
            var items = context.ExpressionStack.Pop(2);
            var value = items[0];
            var address = items[1];

            // For reference types, we want to assign the pointer value itself, not dereference to the object
            // So if address has pointer depth 2 (ref reference type), target depth should be 1
            var targetDepth = address.ResultingType.IsReferenceType && address.PointerDepth == 2 ? 1 : 0;
            var left = new AdjustPointerDepthExpression(address, targetDepth);

            // If the value is coming from a reference type passed by reference, we need to dereference it
            // for the assignment to be valid
            if (value.PointerDepth > 1)
            {
                value = new AdjustPointerDepthExpression(value, 1);
            }
            
            // Check if this is a reference type assignment that needs GC tracking
            var statements = new List<CStatementSet>();
            
            if (address.ResultingType.IsReferenceType && value.ResultingType.IsReferenceType)
            {
                // Untrack the old reference before assignment
                statements.Add(new GcUntrackFunctionCallStatement(left, context.ConversionCatalog));
                
                // Perform the assignment
                statements.Add(new AssignmentStatementSet(left, value));
                
                // Track the new reference after assignment
                statements.Add(new GcTrackFunctionCallStatement(left, context.ConversionCatalog));
            }
            else
            {
                // Non-reference type assignment, just do the assignment
                statements.Add(new AssignmentStatementSet(left, value));
            }

            return new OpCodeHandlingResult(new CompoundStatementSet(statements));
        }

        public OpCodeAnalysisResult Analyze(AnalyzeContext context)
        {
            return new OpCodeAnalysisResult();
        }
    }
}