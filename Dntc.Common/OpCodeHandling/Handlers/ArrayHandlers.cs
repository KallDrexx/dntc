using Dntc.Common.Definitions.CustomDefinedTypes;
using Dntc.Common.Syntax.Expressions;
using Dntc.Common.Syntax.Statements;
using Mono.Cecil.Cil;

namespace Dntc.Common.OpCodeHandling.Handlers;

public class ArrayHandlers : IOpCodeHandlerCollection
{
    public IReadOnlyDictionary<Code, IOpCodeHandler> Handlers { get; } = new Dictionary<Code, IOpCodeHandler>
    {
        { Code.Ldlen, new LdLenHandler() },

        { Code.Ldelema, new LdElemHandler() },
        { Code.Ldelem_I, new LdElemHandler() },
        { Code.Ldelem_I1, new LdElemHandler() },
        { Code.Ldelem_I2, new LdElemHandler() },
        { Code.Ldelem_I4, new LdElemHandler() },
        { Code.Ldelem_I8, new LdElemHandler() },
        { Code.Ldelem_U1, new LdElemHandler() },
        { Code.Ldelem_U2, new LdElemHandler() },
        { Code.Ldelem_R4, new LdElemHandler() },
        { Code.Ldelem_R8, new LdElemHandler() },
        { Code.Ldelem_Any, new LdElemHandler() },
        { Code.Ldelem_Ref, new LdElemHandler() },

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
        public OpCodeHandlingResult Handle(HandleContext context)
        {
            var items = context.ExpressionStack.Pop(3);
            var value = items[0];
            var index = items[1];
            var array = items[2];

            if (array.ResultingType.OriginalTypeDefinition is not ArrayDefinedType arrayDefinedType)
            {
                var message = $"stelem opcode received for an expression returning {array.ResultingType.IlName} " +
                              $"which was defined by {array.ResultingType.OriginalTypeDefinition.GetType().FullName} " +
                              $"which does not inherit from ArrayDefinedType";

                throw new InvalidOperationException(message);
            }
            
            var lengthField = arrayDefinedType.GetArraySizeExpression(array, context.ConversionCatalog);
            var indexExpression = new DereferencedValueExpression(index);

            var itemType = value.ResultingType;
            var itemsExpression = arrayDefinedType.GetItemsAccessorExpression(array, context.ConversionCatalog);
            var arrayIndex = new ArrayIndexExpression(itemsExpression, indexExpression, itemType);
            
            var valueExpression = new DereferencedValueExpression(value);

            var lengthCheck = arrayDefinedType.GetLengthCheckExpression(lengthField, array, indexExpression);
            var storeStatement = new ArrayStoreStatementSet(arrayIndex, valueExpression);

            return new OpCodeHandlingResult(new CompoundStatementSet([lengthCheck, storeStatement]));
        }

        public OpCodeAnalysisResult Analyze(AnalyzeContext context)
        {
            return new OpCodeAnalysisResult();
        }
    }

    private class LdLenHandler : IOpCodeHandler
    {
        public OpCodeHandlingResult Handle(HandleContext context)
        {
            var items = context.ExpressionStack.Pop(1);
            var array = items[0];
            var int32Type = context.ConversionCatalog.Find(new IlTypeName(typeof(int).FullName!));

            var newItem = new FieldAccessExpression(array, new Variable(int32Type, "length", false));
            context.ExpressionStack.Push(newItem);

            return new OpCodeHandlingResult(null);
        }

        public OpCodeAnalysisResult Analyze(AnalyzeContext context)
        {
            return new OpCodeAnalysisResult();
        }
    }

    private class LdElemHandler : IOpCodeHandler
    {
        public OpCodeHandlingResult Handle(HandleContext context)
        {
            var items = context.ExpressionStack.Pop(2);
            var index = items[0];
            var array = items[1];

            if (array.ResultingType.OriginalTypeDefinition is not ArrayDefinedType arrayDefinedType)
            {
                var message = $"ldelem opcode received for an expression returning {array.ResultingType.IlName} " +
                              $"which was defined by {array.ResultingType.OriginalTypeDefinition.GetType().FullName} " +
                              $"which does not inherit from ArrayDefinedType";

                throw new InvalidOperationException(message);
            }

            var lengthField = arrayDefinedType.GetArraySizeExpression(array, context.ConversionCatalog);
            var indexExpression = new DereferencedValueExpression(index);
            var lengthCheck = arrayDefinedType.GetLengthCheckExpression(lengthField, array, indexExpression);
            var itemsExpression = arrayDefinedType.GetItemsAccessorExpression(array, context.ConversionCatalog);
            var arrayIndex = new ArrayIndexExpression(itemsExpression, indexExpression, array.ResultingType);
            
            // Return the length check while adding the accessor expression to the stack
            context.ExpressionStack.Push(arrayIndex);

            return new OpCodeHandlingResult(lengthCheck);
        }

        public OpCodeAnalysisResult Analyze(AnalyzeContext context)
        {
            return new OpCodeAnalysisResult();
        }
    }
}