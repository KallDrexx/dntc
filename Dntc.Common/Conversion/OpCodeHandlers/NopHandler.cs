using Mono.Cecil.Cil;

namespace Dntc.Common.Conversion.OpCodeHandlers;

internal class NopHandler : IOpCodeFnFactory
{
    public IReadOnlyDictionary<Code, OpCodeHandlerFn> Get()
    {
        return new Dictionary<Code, OpCodeHandlerFn>
        {
            { Code.Nop, HandleOpCodeAsync }
        };
    }
    
    private static ValueTask HandleOpCodeAsync(OpCodeHandlingContext context)
    {
        // Do nothing
        return new ValueTask();
    }

}