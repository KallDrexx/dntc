using Dntc.Common;
using Dntc.Common.Conversion;
using Dntc.Common.Conversion.Mutators;
using Dntc.Common.Definitions;

namespace ScratchpadCSharp.Plugin;

public class Aligned8FieldMutator : IFieldConversionMutator
{
    public IReadOnlySet<IlTypeName> RequiredTypes => new HashSet<IlTypeName>();

    public void Mutate(FieldConversionInfo conversionInfo, DotNetDefinedField field)
    {
        var attribute = field.Definition
            .CustomAttributes
            .FirstOrDefault(x => x.AttributeType.FullName == typeof(Aligned8Attribute).FullName);

        if (attribute == null)
        {
            return;
        }

        conversionInfo.AttributeText = "__attribute__ ((aligned (8)))";
    }
}