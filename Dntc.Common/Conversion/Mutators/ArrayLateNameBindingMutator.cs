using Dntc.Common.Definitions;
using Dntc.Common.Definitions.CustomDefinedTypes;

namespace Dntc.Common.Conversion.Mutators;

/// <summary>
/// Allows late binding names for array types. This is needed because we don't have a known
/// name in C for the element type at definition time, we only know them after they've been
/// guaranteed to be added to the definition catalog, so we know the true representation of the
/// element type.
/// </summary>
public class ArrayLateNameBindingMutator : ITypeConversionMutator
{
    private readonly DefinitionCatalog _catalog;
    private readonly ConversionInfoCreator _conversionInfoCreator;

    public ArrayLateNameBindingMutator(DefinitionCatalog catalog, ConversionInfoCreator conversionInfoCreator)
    {
        _catalog = catalog;
        _conversionInfoCreator = conversionInfoCreator;
    }

    public void Mutate(TypeConversionInfo conversionInfo)
    {
        if (conversionInfo.OriginalTypeDefinition is not ArrayDefinedType arrayDefinition)
        {
            return;
        }

        // We can't just pull the type out of the conversion catalog, because we may be
        // creating the array type before the element type. So instead pull the definition
        // out and create the conversion info now.
        var elementDefinition = _catalog.Get(arrayDefinition.ElementType);
        if (elementDefinition == null)
        {
            var message = $"Array type ${arrayDefinition.IlName} uses an element type of " +
                          $"{arrayDefinition.ElementType}, but that type is not in the definition catalog";
            throw new InvalidOperationException(message);
        }

        var elementConversionInfo = _conversionInfoCreator.Create(
            elementDefinition,
            elementDefinition.IlName.IsPointer());

        conversionInfo.NameInC = arrayDefinition.FormTypeName(elementConversionInfo);

        // Update the type definition's name in C for correct code generation.
        arrayDefinition.NativeName = arrayDefinition.FormTypeName(elementConversionInfo);
    }
}