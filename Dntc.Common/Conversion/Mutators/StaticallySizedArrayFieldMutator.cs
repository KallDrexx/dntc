using Dntc.Attributes;
using Dntc.Common.Definitions.CustomDefinedTypes;
using Mono.Cecil;

namespace Dntc.Common.Conversion.Mutators;

public class StaticallySizedArrayFieldMutator : IFieldConversionMutator
{
    private readonly TypeReference _charArrayType;
    private readonly ConversionCatalog _conversionCatalog;

    public StaticallySizedArrayFieldMutator(TypeReference charArrayTypeReference, ConversionCatalog conversionCatalog)
    {
        _charArrayType = charArrayTypeReference;
        _conversionCatalog = conversionCatalog;
    }

    public IReadOnlySet<IlTypeName> RequiredTypes => new HashSet<IlTypeName>([
        new IlTypeName(typeof(char).FullName!),
    ]);

    public void Mutate(FieldConversionInfo conversionInfo, FieldDefinition field)
    {
        var attribute = field
            .CustomAttributes
            .SingleOrDefault(x => x.AttributeType.FullName == typeof(StaticallySizedArrayAttribute).FullName);

        if (attribute == null)
        {
            return;
        }

        if (!field.FieldType.IsArray)
        {
            var message = $"StaticallySizedArrayAttribute was attached to the field {field.FullName} but " +
                          $"its field type is not an array type";

            throw new InvalidOperationException(message);
        }

        if (!attribute.HasConstructorArguments || attribute.ConstructorArguments[0].Value is not int size)
        {
            var message = $"Field {field.FullName}'s StaticallySizedArrayAttribute constructor did not have a " +
                          $"single integer size value";

            throw new InvalidOperationException(message);
        }

        conversionInfo.StaticItemSize = size;
    }
}