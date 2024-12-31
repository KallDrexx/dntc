using Dntc.Attributes;
using Dntc.Common.Definitions.CustomDefinedTypes;
using Dntc.Common.Definitions.Definers;
using Mono.Cecil;

namespace Dntc.Common.Definitions;

public class DefinitionCatalog
{
    private readonly Dictionary<IlTypeName, DefinedType> _types = new();
    private readonly Dictionary<IlMethodId, DefinedMethod> _methods = new();
    private readonly Dictionary<IlFieldId, DefinedGlobal> _globals = new();
    private readonly DefinerDecider _definerDecider;

    public DefinitionCatalog(DefinerDecider decider)
    {
        _definerDecider = decider;
    }

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

    public DefinedType? Get(IlTypeName ilName)
    {
        return _types.GetValueOrDefault(ilName);
    }

    public DefinedMethod? Get(IlMethodId methodId)
    {
        return _methods.GetValueOrDefault(methodId);
    }

    public DefinedGlobal? Get(IlFieldId fieldId)
    {
        return _globals.GetValueOrDefault(fieldId);
    }

    private static NativeDefinedGlobal ConvertToNativeDefinedGlobal(
        FieldDefinition field, 
        CustomAttribute nativeGlobalAttribute)
    {
        if (nativeGlobalAttribute.ConstructorArguments.Count < 1)
        {
            var message = $"NativeGlobalAttribute on '{field.FullName}' expected to have at least " +
                          $"1 constructor arguments but none were found";

            throw new InvalidOperationException(message);
        }

        var hasHeaderSpecified = nativeGlobalAttribute.ConstructorArguments.Count > 1 &&
                                 nativeGlobalAttribute.ConstructorArguments[1].Value != null;

        var header = hasHeaderSpecified
            ? new HeaderName(nativeGlobalAttribute.ConstructorArguments[1].Value.ToString()!)
            : (HeaderName?) null;

        return new NativeDefinedGlobal(
            new IlFieldId(field.FullName),
            new IlTypeName(field.FieldType.FullName),
            new CGlobalName(nativeGlobalAttribute.ConstructorArguments[0].Value.ToString()!),
            header);
    }

    private void Add(TypeDefinition type)
    {
        if (type.Name == "<Module>")
        {
            // Not sure what this is, but it seems safe to ignore
            return;
        }

        var typeDefiner = _definerDecider.GetDefiner(type);
        var definedType = typeDefiner.Define(type);
        Add(definedType);

        foreach (var nestedType in type.NestedTypes)
        {
            Add(nestedType);
        }
        
        foreach (var method in type.Methods)
        {
            var methodDefiner = _definerDecider.GetDefiner(method);
            var definedMethod = methodDefiner.Define(method);
            
            Add(definedMethod);
            if (definedMethod is DotNetDefinedMethod dotNetDefinedMethod)
            {
                foreach (var arrayType in dotNetDefinedMethod.ReferencedArrayTypes)
                {
                    AddReferencedArrayTypes(arrayType);
                }

                foreach (var functionPointer in dotNetDefinedMethod.FunctionPointerTypes)
                {
                    AddDotNetFunctionPointer(functionPointer);
                }
            }
        }

        foreach (var staticField in type.Fields.Where(x => x.IsStatic))
        {
            var nativeGlobalAttribute = staticField.CustomAttributes
                .SingleOrDefault(x => x.AttributeType.FullName == typeof(NativeGlobalAttribute).FullName);

            DefinedGlobal global = nativeGlobalAttribute != null
                ? ConvertToNativeDefinedGlobal(staticField, nativeGlobalAttribute)
                : new DotNetDefinedGlobal(staticField);
            
            Add(global);
        }
    }

    private void AddDotNetFunctionPointer(FunctionPointerType functionPointer)
    {
        var type = new DotNetFunctionPointerType(functionPointer);
        _types.TryAdd(type.IlName, type);
    }

    private void AddReferencedArrayTypes(TypeReference arrayType)
    {
        var type = new ArrayDefinedType(arrayType);
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

    private void Add(DefinedGlobal global)
    {
        if (_globals.TryAdd(global.IlName, global))
        {
            return;
        }
        
        // TODO: Add better duplication logic, and possibly allow overriding
        var message = $"Global '{global.IlName}' is already defined and cannot be redefined";
        throw new InvalidOperationException(message);
    }
}