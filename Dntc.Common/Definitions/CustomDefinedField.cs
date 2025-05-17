using Dntc.Common.Syntax.Statements;
using Mono.Cecil;

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
    
    public FieldDefinition? OriginalField { get; }
    
    protected CustomDefinedField(
        FieldDefinition? originalField,
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
        OriginalField = originalField;

        if (referencedHeaders != null)
        {
            ReferencedHeaders = referencedHeaders;
        }
    }

    /// <summary>
    /// Generates a C statement set representing the declaration of the global or instance field. If left blank
    /// then the normal declaration logic is used.
    /// </summary>
    public abstract CustomCodeStatementSet? GetCustomDeclaration();
}