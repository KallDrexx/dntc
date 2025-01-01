using Dntc.Common.Conversion.Mutators;
using Dntc.Common.Definitions;

namespace Dntc.Common.Conversion;

/// <summary>
/// Creates a ConversionInfo data structure for types, methods, and globals.
/// </summary>
public class ConversionInfoCreator
{
    private readonly List<ITypeConversionMutator> _typeConversionMutators = [];
    private readonly List<IMethodConversionMutator> _methodConversionMutators = [];
    private readonly List<IGlobalConversionMutator> _globalConversionMutators = [];

    public void AddTypeMutator(ITypeConversionMutator mutator)
    {
        _typeConversionMutators.Add(mutator);
    }

    public void AddMethodMutator(IMethodConversionMutator mutator)
    {
        _methodConversionMutators.Add(mutator);
    }

    public void AddGlobalMutator(IGlobalConversionMutator mutator)
    {
        _globalConversionMutators.Add(mutator);
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

    public MethodConversionInfo Create(DefinedMethod method, ConversionCatalog conversionCatalog)
    {
        var conversionInfo = new MethodConversionInfo(method, conversionCatalog);
        if (method is DotNetDefinedMethod dotNetDefinedMethod)
        {
            foreach (var mutator in _methodConversionMutators)
            {
                mutator.Mutate(conversionInfo, dotNetDefinedMethod);
            }
        }

        return conversionInfo;
    }
    
    public GlobalConversionInfo Create(DefinedGlobal global)
    {
        var conversionInfo = new GlobalConversionInfo(global);
        if (global is DotNetDefinedGlobal dotNetDefinedGlobal)
        {
            foreach (var mutator in _globalConversionMutators)
            {
                mutator.Mutate(conversionInfo, dotNetDefinedGlobal);
            }
        }

        return conversionInfo;
    }
}