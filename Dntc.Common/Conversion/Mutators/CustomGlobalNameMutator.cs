using Dntc.Attributes;
using Dntc.Common.Definitions;

namespace Dntc.Common.Conversion.Mutators;

public class CustomGlobalNameMutator : IGlobalConversionMutator
{
    public void Mutate(FieldConversionInfo conversionInfo, DotNetDefinedField field)
    {
        var attribute = field.Definition
            .CustomAttributes
            .FirstOrDefault(x => x.AttributeType.FullName == typeof(CustomFieldNameAttribute).FullName);

        if (attribute == null || attribute.ConstructorArguments.Count < 1)
        {
            return;
        }

        conversionInfo.NameInC = new CFieldName(attribute.ConstructorArguments[0].Value.ToString()!);
    }
}