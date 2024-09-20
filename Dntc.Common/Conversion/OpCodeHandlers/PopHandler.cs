using Mono.Cecil.Cil;

namespace Dntc.Common.Conversion.OpCodeHandlers;

internal class PopHandler : IOpCodeFnFactory
{
    public IReadOnlyDictionary<Code, OpCodeHandlerFn> Get() => new Dictionary<Code, OpCodeHandlerFn>
    {
        { Code.Pop, Handle },
    };

    private static async ValueTask Handle(OpCodeHandlingContext context)
    {
        if (!context.EvaluationStack.TryPop(out var item))
        {
            var message = "Attempted to pop an empty evaluation stack. One item is required";
            throw new InvalidOperationException(message);
        }
        
        // This is usually the case of a method call without storing the result. So just 
        // put the evaluation stack item on its own statement.
        await context.Writer.WriteLineAsync($"\t{item.Text};");
    }
}