using Mono.Cecil.Cil;

namespace Dntc.Common.Conversion.OpCodeHandlers;

internal class MathHandlers : IOpCodeFnFactory
{
    public IReadOnlyDictionary<Code, OpCodeHandlerFn> Get() => new Dictionary<Code, OpCodeHandlerFn>
    {
        { Code.Add, HandleAdd },
        { Code.Sub, HandleSub },
        { Code.Mul, HandelMul },
        { Code.Div, HandleDiv },
        { Code.Rem, HandleRem },
    };

    private static ValueTask HandleAdd(OpCodeHandlingContext context)
    {
        return PushOp("+", context.EvaluationStack);
    }

    private static ValueTask HandelMul(OpCodeHandlingContext context)
    {
        return PushOp("*", context.EvaluationStack);
    }

    private static ValueTask HandleDiv(OpCodeHandlingContext context)
    {
        return PushOp("/", context.EvaluationStack);
    }

    private static ValueTask HandleSub(OpCodeHandlingContext context)
    {
        return PushOp("-", context.EvaluationStack);
    }

    private static ValueTask HandleRem(OpCodeHandlingContext context)
    {
        return PushOp("%", context.EvaluationStack);
    }

    private static ValueTask PushOp(string operatorString, Stack<EvaluationStackItem> evaluationStack)
    {
        var items = evaluationStack.PopCount(2);
        var item2 = items[0];
        var item1 = items[1];
        
        // In theory only values should be put on the stack before a math operation, except for the first item
        // in an add operation. I don't know how common pointer arithmetic actually is though.

        var resultIsPointer = item1.IsPointer || item2.IsPointer; 
        var newItem = new EvaluationStackItem($"({item1.RawText} {operatorString} {item2.RawText})", resultIsPointer);
        evaluationStack.Push(newItem);
        return new ValueTask();
    }
}