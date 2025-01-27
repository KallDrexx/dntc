using Dntc.Attributes;
using Dntc.Common.Definitions;

namespace Dntc.Common.Conversion.Mutators;

public class WithAttributeMutator : IMethodConversionMutator, IFieldConversionMutator
{
    public void Mutate(MethodConversionInfo conversionInfo, DotNetDefinedMethod method)
    {
        var attribute = method.Definition
            .CustomAttributes
            .SingleOrDefault(x => x.AttributeType.FullName == typeof(WithCAttributeAttribute).FullName);

        if (attribute == null || conversionInfo.IsPredeclared)
        {
            return;
        }

        conversionInfo.AttributeText = attribute.ConstructorArguments[0].Value.ToString()!;
    }

    public void Mutate(FieldConversionInfo conversionInfo, DotNetDefinedField field)
    {
        var attribute = field.Definition
            .CustomAttributes
            .SingleOrDefault(x => x.AttributeType.FullName == typeof(WithCAttributeAttribute).FullName);

        if (attribute == null || conversionInfo.IsPredeclared)
        {
            return;
        }

        conversionInfo.AttributeText = attribute.ConstructorArguments[0].Value.ToString();
    }
}