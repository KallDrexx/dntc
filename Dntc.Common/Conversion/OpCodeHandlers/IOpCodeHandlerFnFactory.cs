using Mono.Cecil.Cil;

namespace Dntc.Common.Conversion.OpCodeHandlers;

public interface IOpCodeHandlerFnFactory
{
    IReadOnlyDictionary<Code, OpCodeHandlerFn> Get();
}