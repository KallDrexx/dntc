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

        var fieldType = field.FieldType;
        if (fieldType.FullName == typeof(string).FullName)
        {
            fieldType = _charArrayType;
        }

        if (!fieldType.IsArray)
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
        
        // We need to get the derived type info of the array's element type, so we can
        // accurately tell the array's type conversion info what it's native name is. This
        // *should* not be a race condition because the dependency graph should have ensured
        // that this field's type info is added to the conversion catalog before the field itself.
        var elementType = new IlTypeName(fieldType.GetElementType().FullName);
        var elementTypeInfo = _conversionCatalog.Find(elementType);

        var sizeType = new IlTypeName(typeof(int).FullName!); // TODO: Figure out a way to make this configurable.
        var definedType = new StaticallySizedArrayDefinedType(fieldType, elementTypeInfo, size, sizeType);
        var fieldTypeInfo = new TypeConversionInfo(definedType, false);

        // Replace the field's current conversion info with one based on a statically sized defined type
        conversionInfo.FieldTypeConversionInfo = fieldTypeInfo;
        conversionInfo.StaticItemSize = size;
    }
}