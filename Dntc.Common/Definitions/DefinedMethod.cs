namespace Dntc.Common.Definitions;

public abstract class DefinedMethod
{
    public record Parameter(ClrTypeName Type, string Name);
    
    public ClrMethodId Id { get; protected set; }
    public ClrTypeName ReturnType;
    public IReadOnlyList<Parameter> Parameters { get; protected set; }
    public IReadOnlyList<ClrTypeName> Locals { get; protected set; }
}