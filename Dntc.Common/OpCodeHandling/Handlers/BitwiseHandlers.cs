using Dntc.Common.Conversion;
using Dntc.Common.Syntax.Expressions;
using Mono.Cecil.Cil;

namespace Dntc.Common.OpCodeHandling.Handlers;

public class BitwiseHandlers : IOpCodeHandlerCollection
{
    public IReadOnlyDictionary<Code, IOpCodeHandler> Handlers { get; } = new Dictionary<Code, IOpCodeHandler>
    {
        { Code.Shl, new BitwiseHandler("<<") },
        { Code.Shr, new BitwiseHandler(">>") },
        { Code.Shr_Un, new BitwiseHandler(">>") },
        { Code.And, new BitwiseHandler("&") },
        { Code.Or, new BitwiseHandler("|") },
        { Code.Xor, new BitwiseHandler("^") },
        { Code.Not, new BitwiseHandler("~") },
    };

    private class BitwiseHandler(string @operator) : IOpCodeHandler
    {
        public OpCodeHandlingResult Handle(
            Instruction currentInstruction, 
            ExpressionStack expressionStack,
            MethodConversionInfo currentMethod, 
            ConversionCatalog conversionCatalog)
        {
            var items = expressionStack.Pop(2);
            var right = items[2];
            var left = items[1];

            var newExpression = new TwoExpressionEvalExpression(left, @operator, right);
            expressionStack.Push(newExpression);

            return new OpCodeHandlingResult(null);
        }
    }
}