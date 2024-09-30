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
}