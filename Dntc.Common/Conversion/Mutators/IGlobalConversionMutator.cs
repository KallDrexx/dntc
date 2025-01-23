using Dntc.Common.Definitions;

namespace Dntc.Common.Conversion.Mutators;

public interface IGlobalConversionMutator
{
    void Mutate(FieldConversionInfo conversionInfo, DotNetDefinedField field);
}