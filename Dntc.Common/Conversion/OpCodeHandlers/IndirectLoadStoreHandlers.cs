using Mono.Cecil.Cil;

namespace Dntc.Common.Conversion.OpCodeHandlers;

internal class IndirectLoadStoreHandlers : IOpCodeFnFactory
{
    public IReadOnlyDictionary<Code, OpCodeHandlerFn> Get() => new Dictionary<Code, OpCodeHandlerFn>
    {
        { Code.Ldind_I, HandleLdInd },
        { Code.Ldind_I1, HandleLdInd },
        { Code.Ldind_I2, HandleLdInd },
        { Code.Ldind_I4, HandleLdInd },
        { Code.Ldind_I8, HandleLdInd },
        { Code.Ldind_R4, HandleLdInd },
        { Code.Ldind_R8, HandleLdInd },
        { Code.Ldind_U1, HandleLdInd },
        { Code.Ldind_U2, HandleLdInd },
        { Code.Ldind_U4, HandleLdInd },
        
        { Code.Stind_I, HandleStInd },
        { Code.Stind_I2, HandleStInd },
        { Code.Stind_I4, HandleStInd },
        { Code.Stind_I8, HandleStInd },
        { Code.Stind_R4, HandleStInd },
        { Code.Stind_R8, HandleStInd },
    };

    private static ValueTask HandleLdInd(OpCodeHandlingContext context)
    {
        var items = context.EvaluationStack.PopCount(1);
        if (!items[0].IsPointer)
        {
            var message = $"Expected top of the stack to have a pointer, but it was not: '{items[0].RawText}'";
            throw new InvalidOperationException(message);
        }

        var newItem = new EvaluationStackItem(items[0].Dereferenced, false);
        context.EvaluationStack.Push(newItem);

        return new ValueTask();
    }

    private static async ValueTask HandleStInd(OpCodeHandlingContext context)
    {
        var items = context.EvaluationStack.PopCount(2);
        var value = items[0];
        var address = items[1];
        
        if (!address.IsPointer)
        {
            var message = $"Expected bottom stack item to be a pointer, but it wasn't: '{address.RawText}'";
            throw new InvalidOperationException(message);
        }

        await context.Writer.WriteLineAsync($"\t{address.Dereferenced} = {value.Dereferenced};");
    }
}