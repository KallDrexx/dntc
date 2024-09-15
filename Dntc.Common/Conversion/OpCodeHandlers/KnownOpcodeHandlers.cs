using Mono.Cecil.Cil;

namespace Dntc.Common.Conversion.OpCodeHandlers;

internal class KnownOpcodeHandlers
{
    private readonly IReadOnlyDictionary<Code, OpCodeHandlerFn> _handlers;

    public KnownOpcodeHandlers()
    {
        _handlers = GetType().Assembly
            .GetTypes()
            .Where(x => !x.IsInterface)
            .Where(x => !x.IsAbstract)
            .Where(x => x.IsAssignableTo(typeof(IOpCodeHandlerFnFactory)))
            .Select(Activator.CreateInstance)
            .Cast<IOpCodeHandlerFnFactory>()
            .SelectMany(x => x.Get())
            .ToDictionary(x => x.Key, x => x.Value);
    }

    public OpCodeHandlerFn? Get(Code opCode)
    {
        return _handlers.GetValueOrDefault(opCode);
    }
}