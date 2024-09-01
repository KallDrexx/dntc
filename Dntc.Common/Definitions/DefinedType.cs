namespace Dntc.Common.Definitions;

public abstract class DefinedType
{
    public record Field(ClrTypeName Type, string Name);
    
    public ClrTypeName ClrName { get; protected set; }
    
    public IReadOnlyList<Field> Fields { get; protected set; }
    public IReadOnlyList<ClrMethodId> Methods { get; protected set; }
}