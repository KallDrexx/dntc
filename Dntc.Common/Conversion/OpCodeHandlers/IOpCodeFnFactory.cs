using Mono.Cecil.Cil;

namespace Dntc.Common.Conversion.OpCodeHandlers;

internal interface IOpCodeFnFactory
{
    IReadOnlyDictionary<Code, OpCodeHandlerFn> Get();
}