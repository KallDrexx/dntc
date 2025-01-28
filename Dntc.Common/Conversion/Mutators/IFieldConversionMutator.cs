using Dntc.Common.Definitions;

namespace Dntc.Common.Conversion.Mutators;

public interface IFieldConversionMutator
{
    /// <summary>
    /// Specifies any types that are required to exist in the conversion catalog for this
    /// mutator to function.
    /// </summary>
    IReadOnlySet<IlTypeName> RequiredTypes { get; }
    
    void Mutate(FieldConversionInfo conversionInfo, DotNetDefinedField field);
} 