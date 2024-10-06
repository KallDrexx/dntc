using Mono.Cecil.Cil;

namespace Dntc.Common.OpCodeHandling;

public interface IOpCodeHandlerCollection
{
    IReadOnlyDictionary<Code, IOpCodeHandler> Handlers { get; }
}