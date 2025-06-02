using Dntc.Common.Conversion;
using Dntc.Common.Syntax.Statements;

namespace Dntc.Common.Definitions;

/// <summary>
/// A type that generates custom code to handle a specific type
/// </summary>
public abstract class CustomDefinedType : DefinedType
{
    public HeaderName? HeaderName { get; set; }
    public CSourceFileName? SourceFileName { get; set; }
    public CTypeName NativeName { get; set; }
    public bool IsConsideredDotNetReferenceType { get; protected set; }

    protected CustomDefinedType(
        IlTypeName ilTypeName, 
        HeaderName? headerName, 
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

        InstanceFields = [];
        Methods = [];
    }
    
    public abstract CStatementSet? GetCustomTypeDeclaration(ConversionCatalog catalog);
}