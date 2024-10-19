using Mono.Cecil;

namespace Dntc.Common.Definitions;

public class DotNetDefinedMethod : DefinedMethod
{
    public MethodDefinition Definition { get; }
    public IReadOnlyList<FunctionPointerType> FunctionPointerTypes { get; }
    public IReadOnlyList<TypeReference> ReferencedArrayTypes { get; }
    public IReadOnlyDictionary<string, IlTypeName> GenericArgumentTypes { get; } = new Dictionary<string, IlTypeName>();

    public DotNetDefinedMethod(MethodDefinition definition)
    {
        Definition = definition;
        Id = new IlMethodId(definition.FullName);
        ReturnType = new IlTypeName(definition.ReturnType.FullName);

        var parameters = definition.Parameters
            .OrderBy(x => x.Index)
            .Select(GenerateParameter)
            .ToList();

        if (!definition.IsStatic)
        {
            // If this is an instance method, then the first parameter is always the declaring type
            parameters.Insert(0, new Parameter(new IlTypeName(definition.DeclaringType.FullName), "__this", true));
        }

        Parameters = parameters;
        ReferencedArrayTypes = definition.Parameters
            .Select(x => x.ParameterType)
            .Where(x => x.IsArray)
            .Distinct()
            .ToArray();

        Locals = definition.Body != null
            ? definition.Body
                .Variables
                .OrderBy(x => x.Index)
                .Select(x => new Local(new IlTypeName(x.VariableType.FullName), x.VariableType.IsByReference))
                .ToArray()
            : [];

        // Nested types don't have a namespace on them, so we need to go to the root
        var rootDeclaringType = definition.DeclaringType;
        while (rootDeclaringType.DeclaringType != null)
        {
            rootDeclaringType = rootDeclaringType.DeclaringType;
        }

        Namespace = new IlNamespace(rootDeclaringType.Namespace);

        FunctionPointerTypes = definition.Parameters
            .Select(x => x.ParameterType)
            .OfType<FunctionPointerType>()
            .Concat(definition.Body?.Variables.Select(x => x.VariableType).OfType<FunctionPointerType>() ?? [])
            .ToArray();
    }

    private DotNetDefinedMethod(
        MethodDefinition method, 
        IlMethodId methodId,
        IReadOnlyList<IlTypeName> genericArgumentTypes) : this(method)
    {
        Id = methodId;

        GenericArgumentTypes = genericArgumentTypes
            .Select((arg, index) => new { ParamName = Definition.GenericParameters[index].FullName, ArgName = arg })
            .ToDictionary(x => x.ParamName, x => x.ArgName);
        
        // Replace any referenced generic types with the corresponding argument type
        var genericParameters = Definition.GenericParameters
            .Select((param, index) => new { Name = param.FullName, Index = index })
            .ToDictionary(x => x.Name, x => x.Index);

        if (genericParameters.TryGetValue(ReturnType.Value, out var typeIndex))
        {
            ReturnType = genericArgumentTypes[typeIndex];
        }

        Parameters = Parameters
            .Select(x =>
            {
                if (genericParameters.TryGetValue(x.Type.Value, out typeIndex))
                {
                    return x with { Type = genericArgumentTypes[typeIndex] };
                }

                return x;
            })
            .ToArray();

        Locals = Locals
            .Select(x =>
            {
                if (genericParameters.TryGetValue(x.Type.Value, out typeIndex))
                {
                    return x with { Type = genericArgumentTypes[typeIndex] };
                }

                return x;
            }).ToArray();
    }

    public DotNetDefinedMethod MakeGenericInstance(IlMethodId methodId, IReadOnlyList<IlTypeName> genericArguments)
    {
        return new DotNetDefinedMethod(Definition, methodId, genericArguments);
    }

    private static Parameter GenerateParameter(ParameterDefinition definition)
    {
        if (definition.ParameterType.IsArray)
        {
            // This probably needs to be redone to pass by reference, but it's just a size_t and a pointer for now
            return new Parameter(new IlTypeName(definition.ParameterType.FullName), definition.Name, false);
        }

        return new Parameter(
            new IlTypeName(definition.ParameterType.GetElementType().FullName),
            definition.Name,
            definition.ParameterType.IsByReference);
    }

    protected override IReadOnlyList<IlTypeName> GetReferencedTypesInternal() =>
        ReferencedArrayTypes.Select(x => new IlTypeName(x.FullName))
            .Concat(GenericArgumentTypes.Values)
            .ToArray();
}