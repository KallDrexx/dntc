using Mono.Cecil.Cil;

namespace Dntc.Common.OpCodeHandling;

public static class KnownOpCodeHandlers
{
    public static IReadOnlyDictionary<Code, IOpCodeHandler> OpCodeHandlers { get; } =
        typeof(KnownOpCodeHandlers).Assembly
            .GetTypes()
            .Where(x => !x.IsAbstract)
            .Where(x => !x.IsInterface)
            .Where(x => x.IsAssignableTo(typeof(IOpCodeHandlerCollection)))
            .Select(x => (IOpCodeHandlerCollection) Activator.CreateInstance(x)!)
            .SelectMany(x => x.Handlers)
            .ToDictionary(x => x.Key, x => x.Value);
}