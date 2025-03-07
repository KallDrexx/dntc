namespace Dntc.Common.Definitions;

public class NativeDefinedField : DefinedField
{
    public HeaderName? HeaderFile { get; }
    public CFieldName NativeName { get; }
    
    public NativeDefinedField(
        IlFieldId ilName,
        DefinedType fieldType,
        CFieldName nativeName,
        HeaderName? headerFile)
        : base(ilName, fieldType, true)
    {
        HeaderFile = headerFile;
        NativeName = nativeName;
    }
}