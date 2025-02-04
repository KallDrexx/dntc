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
        private static readonly HashSet<string> BoolOperators = [">", "<", "<=", ">=", "==", "!="];
        
        public OpCodeHandlingResult Handle(
            HandleContext context)
        {
            var items = context.ExpressionStack.Pop(2);
            var right = items[0];
            var left = items[1];

            var resultingType = BoolOperators.Contains(@operator)
                ? context.ConversionCatalog.Find(new IlTypeName(typeof(bool).FullName!))
                : left.ResultingType; // Assume non-bool operators use the same type. This is probably incomplete.

            var newExpression = new TwoExpressionEvalExpression(left, @operator, right, resultingType);
            context.ExpressionStack.Push(newExpression);

            return new OpCodeHandlingResult(null);
        }

        public OpCodeAnalysisResult Analyze(AnalyzeContext context)
        {
            return new OpCodeAnalysisResult();
        }
    }
}