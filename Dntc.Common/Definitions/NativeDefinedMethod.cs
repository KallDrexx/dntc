using Mono.Cecil;

namespace Dntc.Common.Definitions;

public class NativeDefinedMethod : DefinedMethod
{
    private readonly IReadOnlyList<IlTypeName> _parameterTypes;
    private readonly MethodDefinition? _definition;
    
    public CFunctionName NativeName { get; }

    public NativeDefinedMethod(
        IlMethodId methodId, 
        IlTypeName returnType,
        IReadOnlyList<HeaderName> headers, 
        CFunctionName nativeName,
        IlNamespace ilNamespace,
        IReadOnlyList<IlTypeName> parameterTypes,
        MethodDefinition? definition = null)
    {
        _parameterTypes = parameterTypes;
        NativeName = nativeName;
        Id = methodId;
        Namespace = ilNamespace;
        ReturnType = returnType;
        
        // Parameter names don't matter since we won't be generating code for an implementation
        Parameters = parameterTypes.Select(x => new Parameter(x, "a", false, false)).ToArray();
        ReferencedHeaders = headers;
        _definition = definition;

        if (definition != null)
        {
            FunctionPointerTypes = definition.Parameters
                .Select(x => x.ParameterType)
                .OfType<FunctionPointerType>()
                .Concat(definition.Body?.Variables.Select(x => x.VariableType).OfType<FunctionPointerType>() ?? [])
                .ToArray();
        }
    }

    public override DefinedMethod MakeGenericInstance(IlMethodId newId, IReadOnlyList<IlTypeName> genericArguments)
    {
        if (_definition == null)
        {
            var message = $"Cannot invoke MakeGenericClone() on native method {Id} which didn't have a Cecil " +
                          $"method definition";
            throw new InvalidOperationException(message);
        }

        var newParameterTypes = new List<IlTypeName>();
        foreach (var parameterType in _parameterTypes)
        {
            var genericIndex = (int?)null;
            for (var x = 0; x < _definition.GenericParameters.Count; x++)
            {
                var generic = _definition.GenericParameters[x];
                if (generic.FullName == parameterType.GetNonPointerOrRef().Value)
                {
                    genericIndex = x;
                    break;
                }
            }

            newParameterTypes.Add(genericIndex != null ? genericArguments[genericIndex.Value] : parameterType);
        }

        var returnType = ReturnType.GetNonPointerOrRef();
        for (var x = 0; x < _definition.GenericParameters.Count; x++)
        {
            if (_definition.GenericParameters[x].FullName == returnType.Value)
            {
                returnType = genericArguments[x];
            }
        }

        if (ReturnType.IsPointer())
        {
            returnType = returnType.AsPointerType();
        }
        
        return new NativeDefinedMethod(newId, returnType, ReferencedHeaders, NativeName, Namespace, newParameterTypes);
    }

    public static IReadOnlyList<NativeDefinedMethod> StandardMethods { get; } =
    [
        new(
            new IlMethodId("System.Double System.Math::Sqrt(System.Double)"),
            new IlTypeName("System.Double"),
            [new HeaderName("<math.h>")],
            new CFunctionName("sqrt"),
            new IlNamespace("System"),
            [new IlTypeName("System.Double")]),
        
        new(
            new IlMethodId("System.Double System.Math::Atan2(System.Double,System.Double)"),
            new IlTypeName("System.Double"),
            [new HeaderName("<math.h>")],
            new CFunctionName("atan2"),
            new IlNamespace("System"),
            [new IlTypeName("System.Double"), new IlTypeName("System.Double")]),
        
        new(
            new IlMethodId("System.Double System.Math::Cos(System.Double)"),
            new IlTypeName("System.Double"),
            [new HeaderName("<math.h>")],
            new CFunctionName("cos"),
            new IlNamespace("System"),
            [new IlTypeName("System.Double")]),
        
        new(
            new IlMethodId("System.Double System.Math::Sin(System.Double)"),
            new IlTypeName("System.Double"),
            [new HeaderName("<math.h>")],
            new CFunctionName("sin"),
            new IlNamespace("System"),
            [new IlTypeName("System.Double")]),
    ];

    protected override IReadOnlyList<IlTypeName> GetReferencedTypesInternal() => [];
}