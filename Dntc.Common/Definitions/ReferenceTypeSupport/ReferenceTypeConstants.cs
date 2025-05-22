namespace Dntc.Common.Definitions.ReferenceTypeSupport;

public class ReferenceTypeConstants
{
    public static HeaderName HeaderFileName = new("dntc.h");
    public static CSourceFileName SourceFileName = new("dntc.c");
    public static IlNamespace IlNamespace = new("Dntc.System");
    public static IlTypeName ReferenceTypeBaseId = new($"{IlNamespace}.ReferenceTypeBase");
    public static CTypeName ReferenceTypeBaseName = new("DntcReferenceTypeBase");
    public static IlFieldId ReferenceTypeBaseFieldId = new($"{ReferenceTypeBaseId} {IlNamespace}::base");
    public static CFieldName ReferenceTypeBaseFieldName = new("__reference_type_base");
    public static IlMethodId GcTrackMethodId = new($"System.Void {ReferenceTypeBaseId}.GcTrack");
    public static IlMethodId GcUntrackMethodId = new($"System.Void {ReferenceTypeBaseId}.GcUntrack");
    public static CFieldName PrepForFreeFnPointerName = new("PrepForFree");

    public static IlMethodId PrepTypeToFreeMethodId(IlTypeName typeToTrack)
    {
        return new IlMethodId($"{typeof(void).FullName} {typeToTrack}.__PrepForFree()");
    }
}