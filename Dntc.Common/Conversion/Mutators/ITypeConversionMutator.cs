using Dntc.Common.Definitions;

namespace Dntc.Common.Conversion.Mutators;

public interface ITypeConversionMutator
{
    void Mutate(TypeConversionInfo conversionInfo, DotNetDefinedType type);
}