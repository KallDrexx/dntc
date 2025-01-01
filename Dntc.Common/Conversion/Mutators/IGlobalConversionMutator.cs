using Dntc.Common.Definitions;

namespace Dntc.Common.Conversion.Mutators;

public interface IGlobalConversionMutator
{
    void Mutate(GlobalConversionInfo conversionInfo, DotNetDefinedGlobal global);
}