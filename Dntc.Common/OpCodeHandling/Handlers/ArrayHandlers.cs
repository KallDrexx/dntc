using Dntc.Common.Conversion;
using Dntc.Common.Syntax;
using Dntc.Common.Syntax.Expressions;
using Dntc.Common.Syntax.Statements;
using Mono.Cecil.Cil;

namespace Dntc.Common.OpCodeHandling.Handlers;

public class ArrayHandlers : IOpCodeHandlerCollection
{
    public IReadOnlyDictionary<Code, IOpCodeHandler> Handlers { get; } = new Dictionary<Code, IOpCodeHandler>
    {
        { Code.Ldlen, new LdLenHandler() },

        { Code.Stelem_I, new StElemHandler() },
        { Code.Stelem_I1, new StElemHandler() },
        { Code.Stelem_I2, new StElemHandler() },
        { Code.Stelem_I4, new StElemHandler() },
        { Code.Stelem_I8, new StElemHandler() },
        { Code.Stelem_R4, new StElemHandler() },
        { Code.Stelem_R8, new StElemHandler() },
        { Code.Stelem_Any, new StElemHandler() },
        { Code.Stelem_Ref, new StElemHandler() },
    };
    
    private class StElemHandler : IOpCodeHandler
    {
        public OpCodeHandlingResult Handle(
            Instruction currentInstruction, 
            ExpressionStack expressionStack,
            MethodConversionInfo currentMethod, 
            ConversionCatalog conversionCatalog)
        {
            var items = expressionStack.Pop(3);
            var value = items[0];
            var index = items[1];
            var array = items[2];
            var int32Type = conversionCatalog.Find(new IlTypeName(typeof(int).FullName!));
            var itemType = value.ResultingType;

            return new OpCodeHandlingResult(
                new ArrayStoreStatementSet(
                    new FieldAccessExpression(array, new Variable(int32Type, "length", false)),
                    new FieldAccessExpression(array, new Variable(itemType, "items", false)),
                    array,
                    new DereferencedValueExpression(index),
                    new DereferencedValueExpression(value))
            );
        }
    }

    private class LdLenHandler : IOpCodeHandler
    {
        public OpCodeHandlingResult Handle(
            Instruction currentInstruction, 
            ExpressionStack expressionStack,
            MethodConversionInfo currentMethod, 
            ConversionCatalog conversionCatalog)
        {
            var items = expressionStack.Pop(1);
            var array = items[0];
            var int32Type = conversionCatalog.Find(new IlTypeName(typeof(int).FullName!));

            var newItem = new FieldAccessExpression(array, new Variable(int32Type, "length", false));
            expressionStack.Push(newItem);

            return new OpCodeHandlingResult(null);
        }
    }
}