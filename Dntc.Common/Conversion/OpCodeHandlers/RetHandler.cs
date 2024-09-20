using Mono.Cecil.Cil;

namespace Dntc.Common.Conversion.OpCodeHandlers;

internal class RetHandler : IOpCodeFnFactory
{
    public IReadOnlyDictionary<Code, OpCodeHandlerFn> Get() => new Dictionary<Code, OpCodeHandlerFn>
    {
        { Code.Ret, Handle },
    };

    private static async ValueTask Handle(OpCodeHandlingContext context)
    {
        await context.Writer.WriteAsync("\t return");
        
        // I'm assuming it's ok to have more than one item on the stack in some situations
        // when a return is encountered.
        if (context.EvaluationStack.TryPop(out var item))
        {
            await context.Writer.WriteAsync($" {item.Text}");
        }
        
        await context.Writer.WriteLineAsync(";");
    }
}