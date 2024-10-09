using Dntc.Common.Conversion;
using Dntc.Common.Syntax;
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
        { Code.Stobj, new StObjHandler() },
        { Code.Starg, new StArgHandler() },
        { Code.Starg_S, new StArgHandler() },
        
        { Code.Stind_I, new StIndHandler() },
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
    
    private class StFldHandler : IOpCodeHandler
    {
        public OpCodeHandlingResult Handle(
            Instruction currentInstruction, 
            ExpressionStack expressionStack,
            MethodConversionInfo currentMethod, 
            ConversionCatalog conversionCatalog)
        {
            var field = (FieldDefinition)currentInstruction.Operand;
            var items = expressionStack.Pop(2);
            var value = items[0];
            var obj = items[1];

            var left = new FieldAccessExpression(obj, new UntypedVariable(field.Name, field.FieldType.IsPointer));
            var right = new DereferencedValueExpression(value);
            var statement = new AssignmentStatementSet(left, right);

            return new OpCodeHandlingResult(statement);
        }
    }
    
    private class StIndHandler : IOpCodeHandler
    {
        public OpCodeHandlingResult Handle(
            Instruction currentInstruction, 
            ExpressionStack expressionStack,
            MethodConversionInfo currentMethod, 
            ConversionCatalog conversionCatalog)
        {
            var items = expressionStack.Pop(2);
            var value = items[0];
            var address = items[1];

            var left = new DereferencedValueExpression(address);
            var right = new DereferencedValueExpression(value);
            var statement = new AssignmentStatementSet(left, right);

            return new OpCodeHandlingResult(statement);
        }
    }
    
    private class StObjHandler : IOpCodeHandler
    {
        public OpCodeHandlingResult Handle(
            Instruction currentInstruction, 
            ExpressionStack expressionStack,
            MethodConversionInfo currentMethod, 
            ConversionCatalog conversionCatalog)
        {
            var items = expressionStack.Pop(2);
            var objectValue = items[0];
            var address = items[1];

            var left = new DereferencedValueExpression(address);
            var right = new DereferencedValueExpression(objectValue);
            var statement = new AssignmentStatementSet(left, right);

            return new OpCodeHandlingResult(statement);
        }
    }
    
    private class StArgHandler : IOpCodeHandler
    {
        public OpCodeHandlingResult Handle(
            Instruction currentInstruction, 
            ExpressionStack expressionStack,
            MethodConversionInfo currentMethod, 
            ConversionCatalog conversionCatalog)
        {
            var items = expressionStack.Pop(1);
            var value = items[0];

            var argIndex = currentInstruction.Operand switch
            {
                int intOperand => intOperand,
                ParameterDefinition parameterDefinition => parameterDefinition.Index,
                _ => throw new NotSupportedException(currentInstruction.Operand.GetType().FullName),
            };

            if (currentMethod.Parameters.Count < argIndex)
            {
                var message = $"starg on argument index {argIndex} is beyond the argument " +
                              $"count of {currentMethod.Parameters.Count}";
                throw new InvalidOperationException(message);
            }

            var argument = currentMethod.Parameters[argIndex];
            var left = new DereferencedValueExpression(
                new VariableValueExpression(
                    new Variable(argument.ConversionInfo, argument.Name, argument.IsReference)));

            var right = new DereferencedValueExpression(value);
            var statement = new AssignmentStatementSet(left, right);

            return new OpCodeHandlingResult(statement);
        }
    }
    
    private class StLocHandler(int? index) : IOpCodeHandler
    {
        public OpCodeHandlingResult Handle(
            Instruction currentInstruction, 
            ExpressionStack expressionStack,
            MethodConversionInfo currentMethod, 
            ConversionCatalog conversionCatalog)
        {
            var items = expressionStack.Pop(1);

            int localIndex;
            if (index != null)
            {
                localIndex = index.Value;
            }
            else
            {
                localIndex = currentInstruction.Operand switch
                {
                    int intIndex => intIndex,
                    VariableDefinition variableDefinition => variableDefinition.Index,
                    _ => throw new NotSupportedException(currentInstruction.Operand.GetType().FullName),
                };
            }

            if (currentMethod.Locals.Count <= localIndex)
            {
                var message = $"stloc for local index {localIndex} but method only has " +
                              $"{currentMethod.Locals.Count} locals";
                throw new InvalidOperationException(message);
            }

            var local = currentMethod.Locals[localIndex];
            var left = new DereferencedValueExpression(
                new VariableValueExpression(
                    new Variable(local.ConversionInfo, Utils.LocalName(localIndex), local.IsReference)));

            var right = new DereferencedValueExpression(items[0]);
            var statement = new AssignmentStatementSet(left, right);

            return new OpCodeHandlingResult(statement);
        }
    }
}