namespace Dntc.Common.Definitions;

public class NativeDefinedMethod : DefinedMethod
{
    public CFunctionName NativeName { get; }

    public NativeDefinedMethod(
        IlMethodId methodId, 
        IlTypeName returnType,
        IReadOnlyList<HeaderName> headers, 
        CFunctionName nativeName,
        IlNamespace ilNamespace,
        IReadOnlyList<IlTypeName> parameterTypes)
    {
        NativeName = nativeName;
        Id = methodId;
        Namespace = ilNamespace;
        ReturnType = returnType;
        
        // Parameter names don't matter since we won't be generating code for an implementation
        Parameters = parameterTypes.Select(x => new Parameter(x, "a", false)).ToArray();
        ReferencedHeaders = headers;
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