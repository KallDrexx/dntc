using Dntc.Common.Conversion;
using Dntc.Common.Definitions.CustomDefinedMethods;

namespace Dntc.Common.Definitions;

/// <summary>
/// A method that generates source code for a method
/// </summary>
public abstract class CustomDefinedMethod : DefinedMethod
{
    public HeaderName HeaderName { get; }
    public CSourceFileName? SourceFileName { get; }
    public CFunctionName NativeName { get; }

    protected CustomDefinedMethod(
        IlMethodId methodId,
        IlTypeName returnType,
        IlNamespace ilNamespace,
        HeaderName headerName,
        CSourceFileName? sourceFileName,
        CFunctionName nativeName,
        IReadOnlyList<Parameter> parameters)
    {
        Id = methodId;
        ReturnType = returnType;
        Namespace = ilNamespace;
        Parameters = parameters;
        Locals = Array.Empty<Local>();
        HeaderName = headerName;
        SourceFileName = sourceFileName;
        NativeName = nativeName;
    }

    public abstract ValueTask WriteHeaderContentsAsync(StreamWriter writer);

    public abstract ValueTask WriteSourceFileContentsAsync(StreamWriter writer);

    public static IReadOnlyList<CustomDefinedMethod> StandardCustomMethods { get; } =
    [
        new FloatMinDefinedMethod(),
    ];
}