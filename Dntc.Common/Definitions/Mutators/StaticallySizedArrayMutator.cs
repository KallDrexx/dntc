using Dntc.Attributes;
using Dntc.Common.Conversion;
using Dntc.Common.Definitions.CustomDefinedTypes;
using Dntc.Common.Definitions.Definers;
using Mono.Cecil;

namespace Dntc.Common.Definitions.Mutators;

/// <summary>
/// Updates dntc definitions to point to a special IL name designated for statically sized arrays. This is
/// required because we need to differentiate between these and heap allocated arrays (or other arrays).
/// These must be run before heap allocated mutators.
/// </summary>
public class StaticallySizedArrayMutator : IFieldDefinitionMutator
{
    private readonly DefinitionGenerationPipeline _definitionGenerationPipeline;
    private readonly DefinitionCatalog _definitionCatalog;

    public StaticallySizedArrayMutator(
        DefinitionGenerationPipeline definitionGenerationPipeline,
        DefinitionCatalog definitionCatalog)
    {
        _definitionGenerationPipeline = definitionGenerationPipeline;
        _definitionCatalog = definitionCatalog;
    }

    public void Mutate(DefinedField field, FieldDefinition cecilField)
    {
        if (!cecilField.FieldType.IsArray)
        {
            return;
        }

        var attribute = Utils.GetCustomAttribute(typeof(StaticallySizedArrayAttribute), cecilField);
        if (attribute == null)
        {
            return;
        }

        if (!attribute.HasConstructorArguments || attribute.ConstructorArguments[0].Value is not int size)
        {
            var message = $"Field {cecilField.FullName}'s StaticallySizedArrayAttribute constructor did not have a " +
                          $"single integer size value";

            throw new InvalidOperationException(message);
        }

        // We need to get the derived type info of the array's element type, so we can
        // accurately tell the array's type conversion info what it's native name is. Eventually
        // there should be a better way to do this, but this is only needed for arrays.
        var elementTypeDefinition = _definitionGenerationPipeline.Define(cecilField.FieldType.Resolve());
        var elementTypeInfo = new TypeConversionInfo(elementTypeDefinition, cecilField.FieldType.IsPointer);

        var sizeType = new IlTypeName(typeof(int).FullName!); // TODO: Figure out a way to make this configurable.

        // We can't use the real IlName, because we need a custom iL name for this specific usage.
        var ilName = new IlTypeName($"StaticallySizedArray({cecilField.FullName})");
        var definedType = new StaticallySizedArrayDefinedType(cecilField.FieldType, elementTypeInfo, ilName, size, sizeType);
        _definitionCatalog.Add([definedType]);

        // Update the field to use the custom type IL name
        field.IlType = ilName;
    }
}