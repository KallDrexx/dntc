namespace Dntc.Common.Definitions;

public abstract class DefinedField
{
    public IlFieldId IlName { get; }
    public IlTypeName IlType { get; set; }
    public bool IsGlobal { get; }
    public IReadOnlyList<HeaderName> ReferencedHeaders { get; protected set; } = Array.Empty<HeaderName>();
    
    protected DefinedField(IlFieldId name, IlTypeName type, bool isGlobal)
    {
        IlName = name;
        IlType = type;
        IsGlobal = isGlobal;
    }
}