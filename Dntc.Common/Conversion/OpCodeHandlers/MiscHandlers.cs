using Mono.Cecil.Cil;

namespace Dntc.Common.Conversion.OpCodeHandlers;

internal class MiscHandlers : IOpCodeFnFactory
{
    public IReadOnlyDictionary<Code, OpCodeHandlerFn> Get()
    {
        return new Dictionary<Code, OpCodeHandlerFn>
        {
            { Code.Nop, HandleNop },
            { Code.Dup, HandleDup },
        };
    }
    
    private static ValueTask HandleNop(OpCodeHandlingContext context)
    {
        // Do nothing
        return new ValueTask();
    }

    private static ValueTask HandleDup(OpCodeHandlingContext context)
    {
        var items = context.EvaluationStack.PopCount(1);
        context.EvaluationStack.Push(items[0]);
        context.EvaluationStack.Push(items[0].Clone());

        return new ValueTask();
    }
}