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

            CompoundStatementSet statement;
            if (field.IsStatic)
            {
                var items = context.ExpressionStack.Pop(1);
                var value = items[0];

                var variable = new Variable(
                    fieldConversionInfo.FieldTypeConversionInfo,
                    fieldConversionInfo.NameInC.Value, 
                    false);

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

                statement = referenceReplacements != null
                    ? new CompoundStatementSet([referenceReplacements, assignment])
                    : new CompoundStatementSet([assignment]);
            }
            else
            {
                var items = context.ExpressionStack.Pop(2);
                var value = items[0];
                var obj = items[1];

                var left = new FieldAccessExpression(obj,
                    new Variable(value.ResultingType, fieldConversionInfo.NameInC.Value, field.FieldType.IsPointer));

                var right = value;

                // I think this might need to handle referenced variable replacement but using field access expression
                // instead of variable value expressions. I can't get a good use case to reproduce it though, so
                // I'm going to leave it as is for now pending a good test case. In most test cases I've tried, the
                // field loading does not tend to use the dup trick that static fields use to keep a hanging reference
                // on the evaluation stack.
                statement = new CompoundStatementSet([new AssignmentStatementSet(left, right)]);
            }

            return new OpCodeHandlingResult(statement);
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
    }

    private class StIndHandler : IOpCodeHandler
    {
        public OpCodeHandlingResult Handle(HandleContext context)
        {
            var items = context.ExpressionStack.Pop(2);
            var value = items[0];
            var address = items[1];

            var left = new DereferencedValueExpression(address);
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

            var left = new DereferencedValueExpression(address);
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
            var storedVariableExpression = new VariableValueExpression(
                new Variable(argument.ConversionInfo, argument.Name, argument.IsReference));

            var statement = new AssignmentStatementSet(storedVariableExpression, value);

            var tempVariable = HandleReferencedVariable(
                context.ExpressionStack,
                storedVariableExpression,
                context.CurrentInstruction.Offset);

            CStatementSet result = tempVariable != null
                ? new CompoundStatementSet([tempVariable, statement])
                : statement;

            return new OpCodeHandlingResult(result);
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
            var localVariable = new VariableValueExpression(
                new Variable(local.ConversionInfo, Utils.LocalName(context.CurrentDotNetMethod.Definition, localIndex), local.IsReference));

            CBaseExpression left, right;
            if (items[0].ProducesAPointer && localVariable.ProducesAPointer)
            {
                // Both are pointers and thus assignment is compatible as is
                left = localVariable;
                right = items[0];
            }
            else if (localVariable.ProducesAPointer && !items[0].ProducesAPointer)
            {
                // local is a pointer, so we need to dereference it to store the right hand value
                left = new DereferencedValueExpression(localVariable);
                right = items[0];
            }
            else if (!localVariable.ProducesAPointer && items[0].ProducesAPointer)
            {
                if (localVariable.ResultingType.OriginalTypeDefinition is DotNetDefinedType { Definition.IsInterface: true } dotNetDefinedType)
                {
                    left = localVariable;
                    if (localVariable.ResultingType.IlName != items[0].ResultingType.IlName)
                    {
                        right = new InterfaceDynamicCastExpression(localVariable, items[0],
                            dotNetDefinedType.Definition.MetadataToken.RID);
                    }
                    else
                    {
                        right = items[0];
                    }
                }
                else if (items[0].ResultingType.OriginalTypeDefinition is DotNetDefinedType { Definition.IsValueType: false })
                {
                    left = localVariable;
                    right = items[0];
                }
                else
                {
                    // Set the local's value to the dereferenced value of the assigment's expression
                    left = localVariable;
                    right = new DereferencedValueExpression(items[0]);    
                }
                
            }
            else if (!localVariable.ProducesAPointer && !items[0].ProducesAPointer &&
                     localVariable.ResultingType.OriginalTypeDefinition is DotNetDefinedType
                     {
                         Definition.IsInterface: true
                     } dotNetDefinedType)
            {
                left = localVariable;
                if (localVariable.ResultingType.IlName != items[0].ResultingType.IlName)
                {
                    right = new InterfaceDynamicCastExpression(localVariable, items[0],
                        dotNetDefinedType.Definition.MetadataToken.RID);
                }
                else
                {
                    right = items[0];
                }
            }
            else
            {
                // Both are non-pointers
                left = localVariable;
                right = items[0];
            }

            var tempStatement = HandleReferencedVariable(context.ExpressionStack, localVariable,
                context.CurrentInstruction.Offset);

            var statement = new AssignmentStatementSet(left, right);

            CStatementSet result = tempStatement != null
                ? new CompoundStatementSet([tempStatement, statement])
                : statement;

            return new OpCodeHandlingResult(result);
        }

        public OpCodeAnalysisResult Analyze(AnalyzeContext context)
        {
            return new OpCodeAnalysisResult();
        }
    }
}