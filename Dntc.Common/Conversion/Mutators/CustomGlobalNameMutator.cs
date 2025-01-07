using Dntc.Attributes;
using Dntc.Common.Definitions;

namespace Dntc.Common.Conversion.Mutators;

public class CustomGlobalNameMutator : IGlobalConversionMutator
{
    public void Mutate(GlobalConversionInfo conversionInfo, DotNetDefinedGlobal global)
    {
        var attribute = global.Definition
            .CustomAttributes
            .FirstOrDefault(x => x.AttributeType.FullName == typeof(CustomGlobalNameAttribute).FullName);

        if (attribute == null || attribute.ConstructorArguments.Count < 1)
        {
            return;
        }

        conversionInfo.NameInC = new CGlobalName(attribute.ConstructorArguments[0].Value.ToString()!);
    }
}