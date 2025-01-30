using Dntc.Attributes;
using Mono.Cecil;

namespace Dntc.Common.Conversion.Mutators;

public class CustomFieldNameMutator : IFieldConversionMutator
{
    public IReadOnlySet<IlTypeName> RequiredTypes => new HashSet<IlTypeName>();

    public void Mutate(FieldConversionInfo conversionInfo, FieldDefinition field)
    {
        var attribute = field
            .CustomAttributes
            .FirstOrDefault(x => x.AttributeType.FullName == typeof(CustomFieldNameAttribute).FullName);

        if (attribute == null || attribute.ConstructorArguments.Count < 1)
        {
            return;
        }

        conversionInfo.NameInC = new CFieldName(attribute.ConstructorArguments[0].Value.ToString()!);
    }
}