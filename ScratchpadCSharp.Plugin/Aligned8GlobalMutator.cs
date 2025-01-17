using Dntc.Common.Conversion;
using Dntc.Common.Conversion.Mutators;
using Dntc.Common.Definitions;

namespace ScratchpadCSharp.Plugin;

public class Aligned8GlobalMutator : IGlobalConversionMutator
{
    public void Mutate(GlobalConversionInfo conversionInfo, DotNetDefinedGlobal global)
    {
        var attribute = global.Definition
            .CustomAttributes
            .FirstOrDefault(x => x.AttributeType.FullName == typeof(Aligned8Attribute).FullName);

        if (attribute == null)
        {
            return;
        }

        conversionInfo.AttributeText = "__attribute__ ((aligned (8)))";
    }
}