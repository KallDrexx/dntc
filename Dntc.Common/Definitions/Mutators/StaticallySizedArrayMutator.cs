using Dntc.Attributes;
using Dntc.Common.Definitions.CustomDefinedTypes;
using Mono.Cecil;

namespace Dntc.Common.Definitions.Mutators;

/// <summary>
/// Updates dntc definitions to point to a special IL name designated for statically sized arrays. This is
/// required because we need to differentiate between these and heap allocated arrays (or other arrays).
/// These must be run before heap allocated mutators.
/// </summary>
public class StaticallySizedArrayMutator : IFieldDefinitionMutator
{
    private readonly DefinitionCatalog _definitionCatalog;

    public StaticallySizedArrayMutator(DefinitionCatalog definitionCatalog)
    {
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
            // If this isn't a explicitly marked as a statically sized type, but is marked as a native global,
            // then we need to treat it as a statically sized type with no bounds checking (since we have
            // idea of the size)
            if (Utils.GetCustomAttribute(typeof(NativeGlobalAttribute), cecilField) != null)
            {
                SetupDefinedType(field, cecilField, 0, true);
            }

            return;
        }

        if (!attribute.HasConstructorArguments || attribute.ConstructorArguments[0].Value is not int size)
        {
            var message = $"Field {cecilField.FullName}'s StaticallySizedArrayAttribute constructor did not have a " +
                          $"single integer size value";

            throw new InvalidOperationException(message);
        }

        var bypassBoundsCheck = false;
        if (attribute.ConstructorArguments.Count > 1 && attribute.ConstructorArguments[1].Value is bool bypassArg)
        {
            bypassBoundsCheck = bypassArg;
        }

        SetupDefinedType(field, cecilField, size, bypassBoundsCheck);
    }

    private void SetupDefinedType(DefinedField field, FieldDefinition cecilField, int size, bool bypassBoundsCheck)
    {
        var sizeType = new IlTypeName(typeof(int).FullName!); // TODO: Figure out a way to make this configurable.

        // We can't use the real IlName, because we need a custom iL name for this specific usage.
        var ilName = new IlTypeName($"StaticallySizedArray({cecilField.FullName})");
        var definedType = new StaticallySizedArrayDefinedType(
            cecilField.FieldType,
            ilName,
            size,
            sizeType,
            bypassBoundsCheck);

        _definitionCatalog.Add([definedType]);

        // Update the field to use the custom type IL name
        field.IlType = ilName;
    }
}