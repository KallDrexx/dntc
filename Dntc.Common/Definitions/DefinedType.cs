namespace Dntc.Common.Definitions;

public abstract class DefinedType
{
    public record Field(IlTypeName Type, IlFieldId Id);
    
    public IlTypeName IlName { get; protected set; }
    
    public IReadOnlyList<Field> InstanceFields { get; protected set; } = ArraySegment<Field>.Empty;
    public IReadOnlyList<IlMethodId> Methods { get; protected set; } = ArraySegment<IlMethodId>.Empty;
    public IReadOnlyList<IlTypeName> OtherReferencedTypes { get; protected set; } = ArraySegment<IlTypeName>.Empty;
    public IReadOnlyList<HeaderName> ReferencedHeaders { get; protected set; } = ArraySegment<HeaderName>.Empty;
    
    /// <summary>
    /// Headers that are referenced by this method but cannot be inferred from static analysis. This is
    /// mostly required for custom defined types.
    /// </summary>
    public IReadOnlyList<HeaderName> ManuallyReferencedHeaders { get; protected set; } = ArraySegment<HeaderName>.Empty;
}