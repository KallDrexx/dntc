namespace Dntc.Common.Definitions.ReferenceTypeSupport;

/// <summary>
/// Defines how a reference counting implementation is hooked into the dntc pipeline
/// </summary>
public interface IRefCountImplementation
{
    /// <summary>
    /// Adds any required definitions to the definition catalog
    /// </summary>
    void UpdateCatalog(DefinitionCatalog catalog);

    /// <summary>
    /// Adds any fields to the Reference Base Type definition that are required to support the
    /// reference counting implementation.
    /// </summary>
    void AddFieldsToReferenceTypeBase(ReferenceTypeBaseDefinedType typeBaseDefinedType);
}