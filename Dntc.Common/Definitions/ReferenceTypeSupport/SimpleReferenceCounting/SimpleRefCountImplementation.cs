namespace Dntc.Common.Definitions.ReferenceTypeSupport.SimpleReferenceCounting;

/// <summary>
/// Implementation of a simple, non-thread, and non-cyclic reference counter.
/// </summary>
public class SimpleRefCountImplementation : IRefCountImplementation
{
    public void UpdateCatalog(DefinitionCatalog catalog)
    {
        catalog.Add([new ActiveRefCountField()]);
    }

    public void AddFieldsToReferenceTypeBase(ReferenceTypeBaseDefinedType referenceTypeBase)
    {
        referenceTypeBase.AddField(new ActiveRefCountField());
    }
}