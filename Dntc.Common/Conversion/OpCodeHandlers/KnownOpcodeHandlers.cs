using Mono.Cecil.Cil;

namespace Dntc.Common.Conversion.OpCodeHandlers;

public class KnownOpcodeHandlers
{
    private readonly IReadOnlyDictionary<Code, IOpCodeHandler> _handlers;

    public KnownOpcodeHandlers()
    {
        _handlers = GetType().Assembly
            .GetTypes()
            .Where(x => !x.IsInterface)
            .Where(x => !x.IsAbstract)
            .Where(x => x.IsAssignableTo(typeof(IOpCodeHandler)))
            .Select(Activator.CreateInstance)
            .Cast<IOpCodeHandler>()
            .ToDictionary(x => x.OpCode, x => x);
    }

    public IOpCodeHandler? Get(Code opCode)
    {
        return _handlers.GetValueOrDefault(opCode);
    }
}