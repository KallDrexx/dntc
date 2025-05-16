using Dntc.Common.Definitions.CustomDefinedTypes;
using Dntc.Common.Definitions.Definers;
using Mono.Cecil;

namespace Dntc.Common.Definitions;

public class DefinitionCatalog
{
    private readonly Dictionary<IlTypeName, DefinedType> _types = new();
    private readonly Dictionary<IlMethodId, DefinedMethod> _methods = new();
    private readonly Dictionary<IlFieldId, DefinedField> _fields = new();
    private readonly DefinitionGenerationPipeline _definerPipeline;

    public DefinitionCatalog(DefinitionGenerationPipeline definerPipeline)
    {
        _definerPipeline = definerPipeline;
    }
    
    public IEnumerable<DefinedMethod> Methods => _methods.Values;

    /// <summary>
    /// Adds the specified type to the catalog, along with all of its methods and nested types
    /// </summary>
    public void Add(IEnumerable<TypeDefinition> types)
    {
        foreach (var type in types)
        {
            Add(type);
        }
    }

    public void Add(IEnumerable<DefinedType> types)
    {
        foreach (var type in types)
        {
            Add(type);
        }
    }

    public void Add(IEnumerable<DefinedMethod> methods)
    {
        foreach (var method in methods)
        {
            Add(method);
        }
    }

    public void Add(IEnumerable<DefinedField> fields)
    {
        foreach (var field in fields)
        {
            Add(field);
        }
    }

    public DefinedType? Get(IlTypeName ilName)
    {
        return _types.GetValueOrDefault(ilName);
    }

    public DefinedMethod? Get(IlMethodId methodId)
    {
        return _methods.GetValueOrDefault(methodId);
    }

    public DefinedField? Get(IlFieldId fieldId)
    {
        return _fields.GetValueOrDefault(fieldId);
    }

    public IEnumerable<DefinedMethod> GetMethodOverrides(DefinedMethod method)
    {
        if (method is DotNetDefinedMethod { Definition.IsVirtual: true } dntMethod)
        {
            var baseType = dntMethod.Definition.DeclaringType;

            foreach (var type in _types.Values.OfType<DotNetDefinedType>())
            {
                if (type.Definition.IsSubclassOf(baseType))
                {
                    foreach (var derivedMethod in type.Methods.Select(Get).OfType<DotNetDefinedMethod>())
                    {
                        if (derivedMethod.Definition.IsOverrideOf(dntMethod.Definition))
                        {
                            yield return _methods[derivedMethod.Id];
                        }
                    }
                }
            }
        }
    }

    private void Add(TypeDefinition type)
    {
        if (type.Name == "<Module>")
        {
            // Not sure what this is, but it seems safe to ignore
            return;
        }

        var definedType = _definerPipeline.Define(type);
        Add(definedType);

        foreach (var nestedType in type.NestedTypes)
        {
            Add(nestedType);
        }
        
        foreach (var method in type.Methods)
        {
            var definedMethod = _definerPipeline.Define(method);
            Add(definedMethod);

            foreach (var functionPointer in definedMethod.FunctionPointerTypes)
            {
                AddDotNetFunctionPointer(functionPointer);
            }

            if (definedMethod is DotNetDefinedMethod dotNetDefinedMethod)
            {
                foreach (var arrayType in dotNetDefinedMethod.ReferencedArrayTypes)
                {
                    AddReferencedArrayTypes(arrayType);
                }
            }
        }

        foreach (var field in type.Fields)
        {
            var fieldDefinition = _definerPipeline.Define(field);
            Add(fieldDefinition);
        }
    }

    private void AddDotNetFunctionPointer(FunctionPointerType functionPointer)
    {
        var type = new DotNetFunctionPointerType(functionPointer);
        _types.TryAdd(type.IlName, type);
    }

    private void AddReferencedArrayTypes(TypeReference arrayType)
    {
        var type = new HeapArrayDefinedType(arrayType);
        _types.TryAdd(type.IlName, type);
    }
    
    private void Add(DefinedType type)
    {
        if (_types.TryGetValue(type.IlName, out _))
        {
            // TODO: Add better duplication logic, and possibly allow overriding
            // in some circumstances
            var message = $"CLR type '{type.IlName.Value} is already defined and cannot " +
                          $"be redefined";

            throw new InvalidOperationException(message);
        }

        _types[type.IlName] = type;
    }

    private void Add(DefinedMethod method)
    {
        if (_methods.TryGetValue(method.Id, out _))
        {
            // TODO: Add better duplication logic, and possibly allow overriding
            var message = $"CLR method '{method.Id.Value}' is already defined and cannot " +
                          $"be redefined";

            throw new InvalidOperationException(message);
        }

        _methods[method.Id] = method;
    }

    private void Add(DefinedField field)
    {
        if (_fields.TryAdd(field.IlName, field))
        {
            return;
        }
        
        // TODO: Add better duplication logic, and possibly allow overriding
        var message = $"Field '{field.IlName}' is already defined and cannot be redefined";
        throw new InvalidOperationException(message);
    }
}