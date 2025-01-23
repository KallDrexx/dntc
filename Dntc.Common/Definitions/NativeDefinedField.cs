namespace Dntc.Common.Definitions;

public class NativeDefinedField : DefinedField
{
    public HeaderName? HeaderFile { get; }
    public CFieldName NativeName { get; }
    
    public NativeDefinedField(
        IlFieldId ilName, 
        IlTypeName type,
        CFieldName nativeName, 
        HeaderName? headerFile)
        : base(ilName, type, true)
    {
        HeaderFile = headerFile;
        NativeName = nativeName;
    }
}