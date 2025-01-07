using Dntc.Attributes;
using Dntc.Common.Definitions;

namespace Dntc.Common.Conversion.Mutators;

public class NonPointerStringMutator : IGlobalConversionMutator
{
    public void Mutate(GlobalConversionInfo conversionInfo, DotNetDefinedGlobal global)
    {
        var hasAttribute = global.Definition
            .CustomAttributes
            .Any(x => x.AttributeType.FullName == typeof(NonPointerStringAttribute).FullName);

        if (!hasAttribute || global.IlType.Value != typeof(string).FullName)
        {
            return;
        }

        conversionInfo.IsNonPointerString = true;
    }
}