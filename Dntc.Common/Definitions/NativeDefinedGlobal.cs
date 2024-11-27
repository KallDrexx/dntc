namespace Dntc.Common.Definitions;

public class NativeDefinedGlobal : DefinedGlobal
{
    public HeaderName? HeaderFile { get; }
    public CGlobalName NativeName { get; }
    
    public NativeDefinedGlobal(
        IlFieldId ilName, 
        IlTypeName type,
        CGlobalName nativeName, 
        HeaderName? headerFile) 
        : base(ilName, type)
    {
        HeaderFile = headerFile;
        NativeName = nativeName;
    }
}