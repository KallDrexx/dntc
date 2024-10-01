using Mono.Cecil.Cil;

namespace Dntc.Common.Conversion.OpCodeHandlers;

internal class LoadStoreObjectHandlers : IOpCodeFnFactory
{
    public IReadOnlyDictionary<Code, OpCodeHandlerFn> Get() => new Dictionary<Code, OpCodeHandlerFn>
    {
        { Code.Ldobj, HandleLdObj },
        { Code.Stobj, HandleStObj },
    };

    private static ValueTask HandleLdObj(OpCodeHandlingContext context)
    {
        var items = context.EvaluationStack.PopCount(1);
        var objectAddress = items[0];

        if (!objectAddress.IsPointer)
        {
            var message = $"Expected a pointer but '{objectAddress.RawText}' was not one";
            throw new InvalidOperationException(message);
        }

        var newItem = new EvaluationStackItem(objectAddress.Dereferenced, false);
        context.EvaluationStack.Push(newItem);

        return new ValueTask();
    }

    private static async ValueTask HandleStObj(OpCodeHandlingContext context)
    {
        var items = context.EvaluationStack.PopCount(2);
        var objectValue = items[0];
        var address = items[1];

        if (objectValue.IsPointer)
        {
            var message = $"Expected object value '{objectValue.RawText}' to be a value, not a pointer";
            throw new InvalidOperationException(message);
        }

        if (!address.IsPointer)
        {
            var message = $"Expected address '{address.RawText}' to be a pointer, but it was not";
            throw new InvalidOperationException(message);
        }

        await context.Writer.WriteLineAsync($"\t{address.Dereferenced} = {objectValue.RawText};");
    }
}