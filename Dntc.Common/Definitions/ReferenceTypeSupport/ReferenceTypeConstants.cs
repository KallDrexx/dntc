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
    public static IlMethodId RcIncrementMethodId = new($"System.Void {ReferenceTypeBaseId}.RcIncrement");
}