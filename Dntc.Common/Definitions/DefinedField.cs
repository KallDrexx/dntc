namespace Dntc.Common.Definitions;

public abstract class DefinedField
{
    public IlFieldId IlName { get; }
    public DefinedType FieldType { get; }
    public bool IsGlobal { get; }
    public IReadOnlyList<HeaderName> ReferencedHeaders { get; protected set; } = Array.Empty<HeaderName>();
    
    protected DefinedField(IlFieldId name, DefinedType fieldType, bool isGlobal)
    {
        IlName = name;
        IsGlobal = isGlobal;
        FieldType = fieldType;
    }
}