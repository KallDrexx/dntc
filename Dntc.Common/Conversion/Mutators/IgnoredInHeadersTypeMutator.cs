using Dntc.Attributes;
using Dntc.Common.Definitions;

namespace Dntc.Common.Conversion.Mutators;

public class IgnoredInHeadersTypeMutator : ITypeConversionMutator
{
    public void Mutate(TypeConversionInfo conversionInfo, DotNetDefinedType type)
    {
        var ignoredInHeader = type.Definition
            .CustomAttributes
            .Any(x => x.AttributeType.FullName == typeof(IgnoreInHeaderAttribute).FullName);

        if (!ignoredInHeader || conversionInfo.Header == null || conversionInfo.IsPredeclared)
        {
            return;
        }

        conversionInfo.SourceFileName = new CSourceFileName(conversionInfo.Header.Value.Value);
        conversionInfo.Header = null;
    }
}