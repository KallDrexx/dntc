using Dntc.Common.Conversion;
using Dntc.Common.Definitions.CustomDefinedMethods;
using Dntc.Common.Syntax.Statements;

namespace Dntc.Common.Definitions;

/// <summary>
/// A method that generates source code for a method
/// </summary>
public abstract class CustomDefinedMethod : DefinedMethod
{
    public HeaderName HeaderName { get; set; }
    public CSourceFileName? SourceFileName { get; set; }
    public CFunctionName NativeName { get; set; }
    
    public bool HasImplementation { get; }

    protected CustomDefinedMethod(
        IlMethodId methodId,
        IlTypeName returnType,
        IlNamespace ilNamespace,
        HeaderName headerName,
        CSourceFileName? sourceFileName,
        CFunctionName nativeName,
        IReadOnlyList<Parameter> parameters,
        bool hasImplementation = true)
    {
        Id = methodId;
        ReturnType = returnType;
        Namespace = ilNamespace;
        Parameters = parameters;
        Locals = [];
        HeaderName = headerName;
        SourceFileName = sourceFileName;
        NativeName = nativeName;
        HasImplementation = hasImplementation;
    }

    public abstract CustomCodeStatementSet? GetCustomDeclaration(ConversionCatalog catalog);

    public abstract CStatementSet? GetCustomImplementation(ConversionCatalog catalog);

    public static IReadOnlyList<CustomDefinedMethod> StandardCustomMethods { get; } =
    [
        new FloatMinDefinedMethod(),
        new StaticConstructorInitializerDefinedMethod(),
    ];

    protected override IReadOnlyList<IlTypeName> GetReferencedTypesInternal() => [];
}