using Dntc.Common;
using Dntc.Common.Conversion;
using Dntc.Common.Conversion.Mutators;
using Dntc.Common.Definitions;
using Mono.Cecil;

namespace ScratchpadCSharp.Plugin;

public class Aligned8FieldMutator : IFieldConversionMutator
{
    public IReadOnlySet<IlTypeName> RequiredTypes => new HashSet<IlTypeName>();

    public void Mutate(FieldConversionInfo conversionInfo, FieldDefinition field)
    {
        var attribute = field
            .CustomAttributes
            .FirstOrDefault(x => x.AttributeType.FullName == typeof(Aligned8Attribute).FullName);

        if (attribute == null)
        {
            return;
        }

        conversionInfo.AttributeText = "__attribute__ ((aligned (8)))";
    }
}