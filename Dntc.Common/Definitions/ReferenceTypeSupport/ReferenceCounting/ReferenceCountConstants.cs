namespace Dntc.Common.Definitions.ReferenceTypeSupport.ReferenceCounting;

public static class ReferenceCountConstants
{
    public static IlNamespace IlNamespace = new("Dntc.System");
    public static HeaderName HeaderFileName = new("dntc.h");
    public static CSourceFileName SourceFileName = new("dntc.c");
    public static IlTypeName CounterIlTypeName = new($"{IlNamespace}.ReferenceCounter");
    public static CTypeName CounterTypeNativeName = new("dntc_reference_counter");
    public static IlFieldId CounterIlFieldId = new($"{CounterIlTypeName} {IlNamespace}::ReferenceCounter");
    public static CFieldName CounterFieldName = new("referenceCounter");
    public static CFieldName CurrentCountFieldName = new("currentCount");
    public static IlMethodId IncrementMethodIlName = new ($"System.Void {IlNamespace}.ReferenceCounter.Increment()");
    public static CFunctionName IncrementMethodNativeName = new("dntc_reference_counter_increment");
}