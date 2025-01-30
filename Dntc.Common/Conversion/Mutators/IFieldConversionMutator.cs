using Dntc.Common.Definitions;
using Mono.Cecil;

namespace Dntc.Common.Conversion.Mutators;

public interface IFieldConversionMutator
{
    /// <summary>
    /// Specifies any types that are required to exist in the conversion catalog for this
    /// mutator to function.
    /// </summary>
    IReadOnlySet<IlTypeName> RequiredTypes { get; }
    
    void Mutate(FieldConversionInfo conversionInfo, FieldDefinition fieldDefinition);
} 