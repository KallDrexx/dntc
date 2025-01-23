namespace Dntc.Common.Definitions;

public abstract class DefinedField
{
    public IlFieldId IlName { get; }
    public IlTypeName IlType { get; }
    public bool IsGlobal { get; }
    
    protected DefinedField(IlFieldId name, IlTypeName type, bool isGlobal)
    {
        IlName = name;
        IlType = type;
        IsGlobal = isGlobal;
    }
}