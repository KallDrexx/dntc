using Dntc.Common.Conversion;
using Dntc.Common.Syntax.Statements;

namespace Dntc.Common.Definitions;

/// <summary>
/// A type that generates custom code to handle a specific type
/// </summary>
public abstract class CustomDefinedType : DefinedType
{
    public HeaderName HeaderName { get; }
    public CSourceFileName? SourceFileName { get; }
    public CTypeName NativeName { get; }

    protected CustomDefinedType(
        IlTypeName ilTypeName, 
        HeaderName headerName, 
        CSourceFileName? sourceFileName,
        CTypeName nativeName,
        IReadOnlyList<IlTypeName> otherReferencedTypes,
        IReadOnlyList<HeaderName> referencedHeaders)
    {
        IlName = ilTypeName;
        HeaderName = headerName;
        SourceFileName = sourceFileName;
        OtherReferencedTypes = otherReferencedTypes;
        NativeName = nativeName;
        ReferencedHeaders = referencedHeaders;

        Fields = Array.Empty<Field>();
        Methods = Array.Empty<IlMethodId>();
    }
    
    public abstract CustomCodeStatementSet? GetCustomTypeDeclaration(ConversionCatalog catalog);

    public abstract ValueTask WriteHeaderContentsAsync(ConversionCatalog catalog, StreamWriter writer);

    public abstract ValueTask WriteSourceFileContentsAsync(ConversionCatalog catalog, StreamWriter writer);
}