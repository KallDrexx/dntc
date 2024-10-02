using Mono.Cecil.Cil;

namespace Dntc.Common.Conversion.OpCodeHandlers;

internal class ArrayHandlers : IOpCodeFnFactory
{
    public IReadOnlyDictionary<Code, OpCodeHandlerFn> Get() => new Dictionary<Code, OpCodeHandlerFn>
    {
        { Code.Ldlen, HandleLdLen },
        
        { Code.Stelem_I, HandleStElem },
        { Code.Stelem_I1, HandleStElem },
        { Code.Stelem_I2, HandleStElem },
        { Code.Stelem_I4, HandleStElem },
        { Code.Stelem_I8, HandleStElem },
        { Code.Stelem_R4, HandleStElem },
        { Code.Stelem_R8, HandleStElem },
        { Code.Stelem_Any, HandleStElem },
        { Code.Stelem_Ref, HandleStElem },
    };

    private static async ValueTask HandleStElem(OpCodeHandlingContext context)
    {
        var items = context.EvaluationStack.PopCount(3);
        var value = items[0];
        var index = items[1];
        var array = items[2];

        await context.Writer.WriteLineAsync($"\tif ({array.WithAccessor}length <= {index.Dereferenced}) {{");
        await context.Writer.WriteLineAsync($"\t\tprintf(\"Attempted to write to {array}[%ld], " +
                                            $"but only %zu items are in the array\", {index.Dereferenced}, {array.WithAccessor}length);");
        await context.Writer.WriteLineAsync("\t\tabort();");
        await context.Writer.WriteLineAsync("\t}");
        await context.Writer.WriteLineAsync();
        await context.Writer.WriteLineAsync($"\t{array.WithAccessor}items[{index.Dereferenced}] = {value.Dereferenced};");
    }

    private static ValueTask HandleLdLen(OpCodeHandlingContext context)
    {
        var items = context.EvaluationStack.PopCount(1);
        var array = items[0];

        var newItem = new EvaluationStackItem($"({array.WithAccessor}length)", false);
        context.EvaluationStack.Push(newItem);

        return new ValueTask();
    }
}