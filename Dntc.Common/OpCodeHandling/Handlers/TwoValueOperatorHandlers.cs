using Dntc.Common.Conversion;
using Dntc.Common.Syntax.Expressions;
using Mono.Cecil.Cil;

namespace Dntc.Common.OpCodeHandling.Handlers;

public class TwoValueOperatorHandlers : IOpCodeHandlerCollection
{
    public IReadOnlyDictionary<Code, IOpCodeHandler> Handlers { get; } = new Dictionary<Code, IOpCodeHandler>
    {
        { Code.Shl, new OpCodeHandler("<<") },
        { Code.Shr, new OpCodeHandler(">>") },
        { Code.Shr_Un, new OpCodeHandler(">>") },
        { Code.And, new OpCodeHandler("&") },
        { Code.Or, new OpCodeHandler("|") },
        { Code.Xor, new OpCodeHandler("^") },
        { Code.Not, new OpCodeHandler("~") },

        { Code.Clt, new OpCodeHandler("<") },
        { Code.Clt_Un, new OpCodeHandler("<") },
        { Code.Cgt, new OpCodeHandler(">") },
        { Code.Cgt_Un, new OpCodeHandler(">") },
        { Code.Ceq, new OpCodeHandler("==") },
        
        { Code.Add, new OpCodeHandler("+") },
        { Code.Sub, new OpCodeHandler("-") },
        { Code.Mul, new OpCodeHandler("*") },
        { Code.Div, new OpCodeHandler("/") },
        { Code.Rem, new OpCodeHandler("%") },
    };
    
    private class OpCodeHandler(string @operator) : IOpCodeHandler
    {
        public OpCodeHandlingResult Handle(
            Instruction currentInstruction, 
            ExpressionStack expressionStack,
            MethodConversionInfo currentMethod, 
            ConversionCatalog conversionCatalog)
        {
            var items = expressionStack.Pop(2);
            var right = new DereferencedValueExpression(items[1]);
            var left = new DereferencedValueExpression(items[0]);

            var newExpression = new TwoExpressionEvalExpression(left, @operator, right);
            expressionStack.Push(newExpression);

            return new OpCodeHandlingResult(null);
        }
    }
}