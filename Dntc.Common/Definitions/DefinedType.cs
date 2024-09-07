namespace Dntc.Common.Definitions;

public abstract class DefinedType
{
    public record Field(IlTypeName Type, string Name);
    
    public IlTypeName IlName { get; protected set; }
    
    public IReadOnlyList<Field> Fields { get; protected set; } = ArraySegment<Field>.Empty;
    public IReadOnlyList<IlMethodId> Methods { get; protected set; } = ArraySegment<IlMethodId>.Empty;
}