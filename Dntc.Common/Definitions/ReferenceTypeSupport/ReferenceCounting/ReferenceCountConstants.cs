namespace Dntc.Common.Definitions.ReferenceTypeSupport.ReferenceCounting;

public static class ReferenceCountConstants
{
    public const string IlNamespace = "Dntc.System";
    public const string HeaderFileName = "dntc.h";
    public const string SourceFileName = "dntc.c";
    public const string CounterIlTypeName = $"{IlNamespace}.ReferenceCounter";
    public const string CounterTypeNativeName = "dntc_reference_counter";
    public const string CountFieldName = "currentCount";
    public const string IncrementMethodIlName = $"System.Void {IlNamespace}.ReferenceCounter.Increment()";
    public const string IncrementMethodNativeName = "dntc_reference_counter_increment";
}