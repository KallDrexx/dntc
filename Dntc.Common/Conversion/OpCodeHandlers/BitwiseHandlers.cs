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
        var items = context.EvaluationStack.PopCount(2);
        var amount = items[0];
        var value = items[1];
        
        var newItem = new EvaluationStackItem($"({value.Dereferenced} {bitwiseOperator} {amount.Dereferenced})", false);
        context.EvaluationStack.Push(newItem);

        return new ValueTask();
    }
}