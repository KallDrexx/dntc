namespace Dntc.Common.Definitions;

public class NativeDefinedMethod : DefinedMethod
{
    public HeaderName HeaderFile { get; }
    public CFunctionName NativeName { get; }

    public NativeDefinedMethod(
        IlMethodId methodId, 
        IlTypeName returnType,
        HeaderName headerFile, 
        CFunctionName nativeName,
        IlNamespace ilNamespace,
        IReadOnlyList<IlTypeName> parameterTypes)
    {
        HeaderFile = headerFile;
        NativeName = nativeName;
        Id = methodId;
        Namespace = ilNamespace;
        ReturnType = returnType;
        
        // Parameter names don't matter since we won't be generating code for an implementation
        Parameters = parameterTypes.Select(x => new Parameter(x, "a", false)).ToArray();
    }

    public static IReadOnlyList<NativeDefinedMethod> StandardMethods { get; } =
    [
        new NativeDefinedMethod(
            new IlMethodId("System.Double System.Math::Sqrt(System.Double)"),
            new IlTypeName("System.Double"),
            new HeaderName("<math.h>"),
            new CFunctionName("sqrt"),
            new IlNamespace("System"),
            [new IlTypeName("System.Double")])
    ];
}