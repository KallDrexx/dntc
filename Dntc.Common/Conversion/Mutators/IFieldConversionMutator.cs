using Dntc.Common.Definitions;

namespace Dntc.Common.Conversion.Mutators;

public interface IFieldConversionMutator
{
    void Mutate(FieldConversionInfo conversionInfo, DotNetDefinedField field);
}