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

    private static ValueTask PushOp(string operatorString, Stack<EvaluationStackItem> evaluationStack)
    {
        var items = evaluationStack.PopCount(2);
        var item2 = items[0];
        var item1 = items[1];

        var newItem = new EvaluationStackItem($"({item1.Text} {operatorString} {item2.Text})");
        evaluationStack.Push(newItem);
        return new ValueTask();
    }
}