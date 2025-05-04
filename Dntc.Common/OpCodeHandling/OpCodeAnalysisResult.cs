using Mono.Cecil;

namespace Dntc.Common.OpCodeHandling;

public class OpCodeAnalysisResult
{
    public IReadOnlyList<InvokedMethod> CalledMethods { get; init; } = [];
    public IReadOnlySet<IlTypeName> ReferencedTypes { get; init; } = new HashSet<IlTypeName>();
    public IReadOnlySet<HeaderName> ReferencedHeaders { get; init; } = new HashSet<HeaderName>();
    public FieldDefinition? ReferencedGlobal { get; init; }
}
