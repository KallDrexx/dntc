namespace Dntc.Common.Definitions;

public abstract class DefinedGlobal
{
    public IlFieldId IlName { get; }
    public IlTypeName IlType { get; }
    
    protected DefinedGlobal(IlFieldId name, IlTypeName type)
    {
        IlName = name;
        IlType = type;
    }
}