using Dntc.Attributes;
using Dntc.Common.Definitions.CustomDefinedTypes;
using Mono.Cecil;

namespace Dntc.Common.Definitions;

public class DefinitionCatalog
{
    private readonly Dictionary<IlTypeName, DefinedType> _types = new();
    private readonly Dictionary<IlMethodId, DefinedMethod> _methods = new();
    private readonly Dictionary<IlFieldId, DefinedGlobal> _globals = new();

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

    private void Add(TypeDefinition type)
    {
        if (type.Name == "<Module>")
        {
            // Not sure what this is, but it seems safe to ignore
            return;
        }

        var definedType = new DotNetDefinedType(type);
        Add(definedType);

        foreach (var nestedType in type.NestedTypes)
        {
            Add(nestedType);
        }
        
        foreach (var method in type.Methods)
        {
            var nativeTranspileAttribute = method.CustomAttributes
                .SingleOrDefault(x => x.AttributeType.FullName == typeof(NativeFunctionCallAttribute).FullName);

            if (nativeTranspileAttribute != null)
            {
                if (nativeTranspileAttribute.ConstructorArguments.Count < 1)
                {
                    var message = $"NativeTranspileAttribute on '{method.FullName}' expected to have at least " +
                                  $"1 constructor arguments but none were found";

                    throw new InvalidOperationException(message);
                }

                var hasHeaderSpecified = nativeTranspileAttribute.ConstructorArguments.Count > 1 &&
                                         nativeTranspileAttribute.ConstructorArguments[1].Value != null;

                var header = hasHeaderSpecified
                    ? new HeaderName(nativeTranspileAttribute.ConstructorArguments[1].Value.ToString()!)
                    : (HeaderName?) null;
                
                var nativeDefinedMethod = new NativeDefinedMethod(
                    new IlMethodId(method.FullName),
                    new IlTypeName(method.ReturnType.FullName),
                    header,
                    new CFunctionName(nativeTranspileAttribute.ConstructorArguments[0].Value.ToString()!),
                    new IlNamespace(method.DeclaringType.Namespace),
                    method.Parameters.Select(x => new IlTypeName(x.ParameterType.FullName)).ToArray());

                Add(nativeDefinedMethod);
                continue;
            }
            
            var definedMethod = new DotNetDefinedMethod(method);
            Add(definedMethod);

            foreach (var arrayType in definedMethod.ReferencedArrayTypes)
            {
                AddReferencedArrayTypes(arrayType);
            }
            
            foreach (var functionPointer in definedMethod.FunctionPointerTypes)
            {
                AddDotNetFunctionPointer(functionPointer);
            }
        }

        foreach (var staticField in type.Fields.Where(x => x.IsStatic))
        {
            var global = new DotNetDefinedGlobal(staticField);
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