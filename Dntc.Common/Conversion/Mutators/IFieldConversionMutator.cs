using Dntc.Common.Definitions;
using Mono.Cecil;

namespace Dntc.Common.Conversion.Mutators;

public interface IFieldConversionMutator
{
    void Mutate(FieldConversionInfo conversionInfo, FieldDefinition fieldDefinition);
} 