using Dntc.Common.Conversion;
using Dntc.Common.Syntax.Statements;

namespace Dntc.Common.Definitions;

/// <summary>
/// A type that generates custom code for declaring a field
/// </summary>
public abstract class CustomDefinedField : DefinedField
{
    /// <summary>
    /// The header the field is declared in. Required for globals, not used for instance fields
    /// </summary>
    public HeaderName? DeclaredInHeader { get; }
   
    /// <summary>
    /// The source file the field is declared in. Required for globals, not used for instance fields
    /// </summary>
    public CSourceFileName? DeclaredInSourceFileName { get; }
   
    /// <summary>
    /// The name the field is referred to by in c code.
    /// </summary>
    public CFieldName NativeName { get; }
    
    protected CustomDefinedField(
        HeaderName? declaredInHeader, 
        CSourceFileName? declaredInSourceFileName,
        CFieldName nativeName, 
        IlFieldId name, 
        IlTypeName type, 
        bool isGlobal,
        IReadOnlyList<HeaderName>? referencedHeaders = null) : base(name, type, isGlobal)
    {
        DeclaredInHeader = declaredInHeader;
        DeclaredInSourceFileName = declaredInSourceFileName;
        NativeName = nativeName;

        if (referencedHeaders != null)
        {
            ReferencedHeaders = referencedHeaders;
        }
    }

    public abstract CustomCodeStatementSet GetCustomDeclaration(ConversionCatalog catalog);
}