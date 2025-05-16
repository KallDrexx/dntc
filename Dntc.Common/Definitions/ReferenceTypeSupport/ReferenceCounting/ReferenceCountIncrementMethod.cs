namespace Dntc.Common.Definitions.ReferenceTypeSupport.ReferenceCounting;

/// <summary>
/// Manages incrementing the reference
/// </summary>
public class ReferenceCountIncrementMethod : CustomDefinedMethod
{
    public ReferenceCountIncrementMethod()
        : base(
            new IlMethodId(ReferenceCountConstants.IncrementMethodIlName),
            new IlTypeName(typeof(void).FullName!),
            new IlNamespace(ReferenceCountConstants.IlNamespace),
            new HeaderName(ReferenceCountConstants.HeaderFileName),
            new CSourceFileName(ReferenceCountConstants.SourceFileName),
            new CFunctionName(ReferenceCountConstants.IncrementMethodNativeName),
            [
                new Parameter(new IlTypeName(ReferenceCountConstants.CounterIlTypeName))],
            hasImplementation)
    {
    }
}