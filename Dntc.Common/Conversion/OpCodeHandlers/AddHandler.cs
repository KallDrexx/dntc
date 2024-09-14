using Mono.Cecil.Cil;

namespace Dntc.Common.Conversion.OpCodeHandlers;

public class AddHandler : IOpCodeHandlerFnFactory
{
    public IReadOnlyDictionary<Code, OpCodeHandlerFn> Get()
    {
        
    }

    private static ValueTask HandleAdd(OpCodeHandlingContext context)
    {
        context.
    }
}