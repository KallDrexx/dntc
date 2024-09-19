using Mono.Cecil.Cil;

namespace Dntc.Common.Conversion.OpCodeHandlers;

internal class AddHandler : IOpCodeFnFactory
{
    public IReadOnlyDictionary<Code, OpCodeHandlerFn> Get() => new Dictionary<Code, OpCodeHandlerFn>
    {
        { Code.Add, HandleAdd }
    };

    private static ValueTask HandleAdd(OpCodeHandlingContext context)
    {
        if (context.EvaluationStack.Count < 2)
        {
            const string message = "Add requires two evaluation stack items, only one is present";
            throw new InvalidOperationException(message);
        }

        if (context.EvaluationStack.Count < 2)
        {
            var message = $"Expected at least 2 items on the evaluation stack, but " +
                          $"only {context.EvaluationStack.Count} are in it";
            throw new InvalidOperationException(message);
        }

        var item2 = context.EvaluationStack.Pop();
        var item1 = context.EvaluationStack.Pop();

        var newItem = new EvaluationStackItem($"({item1.Text} + {item2.Text})");
        context.EvaluationStack.Push(newItem);

        return new ValueTask();
    }
}