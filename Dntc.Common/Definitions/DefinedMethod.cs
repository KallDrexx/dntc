namespace Dntc.Common.Definitions;

public abstract class DefinedMethod
{
    public record Parameter(IlTypeName Type, string Name, bool IsReference);

    public record Local(IlTypeName Type, bool IsReference);
    
    public IlMethodId Id { get; protected set; }
    public IlTypeName ReturnType { get; protected set; }
    public IlNamespace Namespace { get; protected set; }
    public IReadOnlyList<Parameter> Parameters { get; protected set; } = ArraySegment<Parameter>.Empty;
    public IReadOnlyList<Local> Locals { get; protected set; } = ArraySegment<Local>.Empty;
    
    /// <summary>
    /// Headers that are referenced by this method but cannot be inferred from static analysis. This is
    /// mostly required for custom defined types.
    /// </summary>
    public IReadOnlyList<HeaderName> ReferencedHeaders { get; protected set; } = ArraySegment<HeaderName>.Empty;
    
    public IReadOnlyList<IlTypeName> GetReferencedTypes => Locals.Select(x => x.Type)
        .Concat(Parameters.Select(x => x.Type))
        .Concat([ReturnType])
        .Concat(GetReferencedTypesInternal())
        .ToArray();

    protected abstract IReadOnlyList<IlTypeName> GetReferencedTypesInternal();
}