using Dntc.Common.Definitions;
using Dntc.Common.Definitions.CustomDefinedMethods;
using Dntc.Common.Definitions.CustomDefinedTypes;
using Dntc.Common.Definitions.ReferenceTypeSupport;

namespace Dntc.Common.Conversion.Mutators;

/// <summary>
/// Allows late binding names for array types. This is needed because we don't have a known
/// name in C for the element type at definition time, we only know them after they've been
/// guaranteed to be added to the definition catalog, so we know the true representation of the
/// element type.
/// </summary>
public class ArrayLateNameBindingMutator : ITypeConversionMutator, IMethodConversionMutator
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

    public void Mutate(MethodConversionInfo conversionInfo, DotNetDefinedMethod? method)
    {
        switch (conversionInfo.OriginalMethodDefinition)
        {
            case ReferenceTypeAllocationMethod allocationMethod:
            {
                if (allocationMethod.TypeReference.IsArray)
                {
                    // Make sure the allocation method's native name matches the native name of the array.
                    var type = _catalog.Get(new IlTypeName(allocationMethod.TypeReference.FullName));
                    if (type is CustomDefinedType customDefinedType)
                    {
                        allocationMethod.NativeName = ReferenceTypeAllocationMethod.FormNativeName(
                            customDefinedType.NativeName.Value);

                        conversionInfo.NameInC = allocationMethod.NativeName;
                    }
                }

                break;
            }

            case PrepToFreeDefinedMethod prepToFreeMethod:
            {
                if (prepToFreeMethod.DefinedType is ArrayDefinedType)
                {
                    // PrepToFreeDefinedMethods are bound to a type definition that's not the exact one from
                    // the definition catalog, due to it being generated in the `newarr` opcode handler. Therefore,
                    // it will not have the correct native name, as it won't have definition mutators run. Thus we
                    // need to get the version from the definition catalog to properly rebind its properties

                    var type = _catalog.Get(prepToFreeMethod.DefinedType.IlName);
                    if (type is CustomDefinedType customDefinedType)
                    {
                        prepToFreeMethod.NativeName = PrepToFreeDefinedMethod.FormNativeName(
                            customDefinedType.NativeName.Value);

                        conversionInfo.NameInC = prepToFreeMethod.NativeName;
                    }
                }

                break;
            }
        }
    }
}