using Mono.Cecil.Cil;

namespace Dntc.Common.Conversion.OpCodeHandlers;

internal class BitwiseHandlers : IOpCodeFnFactory
{
    public IReadOnlyDictionary<Code, OpCodeHandlerFn> Get() => new Dictionary<Code, OpCodeHandlerFn>
    {
        { Code.Shl, CreateBitwiseFn("<<") },
        { Code.Shr, CreateBitwiseFn(">>") },
        { Code.Shr_Un, CreateBitwiseFn(">>") },
        { Code.And, CreateBitwiseFn("&") },
        { Code.Or, CreateBitwiseFn("|") },
        { Code.Xor, CreateBitwiseFn("^") },
        { Code.Not, CreateBitwiseFn("~") },
    };

    private static OpCodeHandlerFn CreateBitwiseFn(string bitwiseOperator)
    {
        return context => BitwiseHandler(bitwiseOperator, context);
    }

    private static ValueTask BitwiseHandler(string bitwiseOperator, OpCodeHandlingContext context)
    {
        if (context.EvaluationStack.Count < 2)
        {
            var message = $"Bitwise operator requires 2 items on the stack, but " +
                          $"only {context.EvaluationStack.Count} are on it";
            throw new InvalidOperationException(message);
        }

        var amount = context.EvaluationStack.Pop();
        var value = context.EvaluationStack.Pop();
        
        var newItem = new EvaluationStackItem($"({value.Text} {bitwiseOperator} {amount.Text})");
        context.EvaluationStack.Push(newItem);

        return new ValueTask();
    }
}