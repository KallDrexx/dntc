namespace Dntc.Common.Definitions.ReferenceTypeSupport.SimpleReferenceCounting;

/// <summary>
/// Implementation of a simple, non-thread, and non-cyclic reference counter.
/// </summary>
public class SimpleRefCountImplementation : IRefCountImplementation
{
    private readonly IMemoryManagementActions _memoryManagement;

    public SimpleRefCountImplementation(IMemoryManagementActions memoryManagement)
    {
        _memoryManagement = memoryManagement;
    }

    public void UpdateCatalog(DefinitionCatalog catalog)
    {
        catalog.Add([new ActiveRefCountField()]);
        catalog.Add([
            new RefCountTrackMethod(_memoryManagement),
            new RefCountUntrackMethod(_memoryManagement),
        ]);
    }

    public void AddFieldsToReferenceTypeBase(ReferenceTypeBaseDefinedType referenceTypeBase)
    {
        referenceTypeBase.AddField(new ActiveRefCountField());
    }
}