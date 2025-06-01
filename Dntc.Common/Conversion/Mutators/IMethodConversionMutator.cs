using Dntc.Common.Definitions;

namespace Dntc.Common.Conversion.Mutators;

public interface IMethodConversionMutator
{
    void Mutate(MethodConversionInfo conversionInfo, DotNetDefinedMethod? method);
}