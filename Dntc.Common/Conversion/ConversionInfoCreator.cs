using Dntc.Common.Conversion.Mutators;
using Dntc.Common.Definitions;
using Mono.Cecil;

namespace Dntc.Common.Conversion;

/// <summary>
/// Creates a ConversionInfo data structure for types, methods, and globals.
/// </summary>
public class ConversionInfoCreator
{
    private readonly List<ITypeConversionMutator> _typeConversionMutators = [];
    private readonly List<IMethodConversionMutator> _methodConversionMutators = [];
    private readonly List<IFieldConversionMutator> _fieldConversionMutators = [];

    public IReadOnlySet<IlTypeName> RequiredTypes
    {
        get
        {
            var set = new HashSet<IlTypeName>();
            foreach (var type in _fieldConversionMutators.SelectMany(mutator => mutator.RequiredTypes))
            {
                set.Add(type);
            }

            return set;
        }
    }

    public void AddTypeMutator(ITypeConversionMutator mutator)
    {
        _typeConversionMutators.Add(mutator);
    }

    public void AddMethodMutator(IMethodConversionMutator mutator)
    {
        _methodConversionMutators.Add(mutator);
    }

    public void AddFieldMutator(IFieldConversionMutator mutator)
    {
        _fieldConversionMutators.Add(mutator);
    }

    public TypeConversionInfo Create(DefinedType type, bool isPointer)
    {
        var conversionInfo = new TypeConversionInfo(type, isPointer);
        foreach (var mutator in _typeConversionMutators)
        {
            mutator.Mutate(conversionInfo);
        }

        return conversionInfo;
    }

    public MethodConversionInfo Create(DefinedMethod method, ConversionCatalog conversionCatalog)
    {
        var conversionInfo = new MethodConversionInfo(method, conversionCatalog);
        var dotNetDefinedMethod = method as DotNetDefinedMethod;

        foreach (var mutator in _methodConversionMutators)
        {
            mutator.Mutate(conversionInfo, dotNetDefinedMethod);
        }
        return conversionInfo;
    }
    
    public FieldConversionInfo Create(DefinedField field, TypeConversionInfo fieldType)
    {
        var conversionInfo = new FieldConversionInfo(field, fieldType);

        var fieldDefinition = field switch
        {
            DotNetDefinedField dotNetDefinedField => dotNetDefinedField.Definition,
            CustomDefinedField customDefinedField => customDefinedField.OriginalField,
            _ => null
        };

        if (fieldDefinition != null)
        {
            foreach (var mutator in _fieldConversionMutators)
            {
                mutator.Mutate(conversionInfo, fieldDefinition);
            }
        }

        return conversionInfo;
    }
}