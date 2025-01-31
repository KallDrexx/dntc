using Dntc.Attributes;
using Dntc.Common.Definitions;
using Mono.Cecil;

namespace Dntc.Common.Conversion.Mutators;

public class NonPointerStringMutator : IFieldConversionMutator
{
    public IReadOnlySet<IlTypeName> RequiredTypes => new HashSet<IlTypeName>();

    public void Mutate(FieldConversionInfo conversionInfo, FieldDefinition field)
    {
        var hasAttribute = field
            .CustomAttributes
            .Any(x => x.AttributeType.FullName == typeof(NonPointerStringAttribute).FullName);

        if (!hasAttribute || field.FieldType.FullName != typeof(string).FullName)
        {
            return;
        }

        conversionInfo.IsNonPointerString = true;
    }
}