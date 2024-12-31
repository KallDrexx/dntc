using Dntc.Common.Conversion.Mutators;
using Dntc.Common.Definitions;

namespace Dntc.Common.Conversion;

/// <summary>
/// Creates a ConversionInfo data structure for types, methods, and globals.
/// </summary>
public class ConversionInfoCreator
{
    private readonly List<ITypeConversionMutator> _typeConversionMutators = [];

    public void Add(ITypeConversionMutator mutator)
    {
        _typeConversionMutators.Add(mutator);
    }

    public TypeConversionInfo Create(DefinedType type)
    {
        var conversionInfo = new TypeConversionInfo(type);
        if (type is DotNetDefinedType dotNetDefinedType)
        {
            foreach (var mutator in _typeConversionMutators)
            {
                mutator.Mutate(conversionInfo, dotNetDefinedType);
            }
        }

        return conversionInfo;
    }
}