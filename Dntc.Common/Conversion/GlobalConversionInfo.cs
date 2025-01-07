using Dntc.Attributes;
using Dntc.Common.Definitions;
using Dntc.Common.Syntax.Expressions;

namespace Dntc.Common.Conversion;

/// <summary>
/// Contains information for how a global will be converted to C
/// </summary>
public class GlobalConversionInfo
{
    /// <summary>
    /// The name this global has when referenced in .net
    /// </summary>
    public IlFieldId IlName { get; }
   
    /// <summary>
    /// The type of value this global contains
    /// </summary>
    public IlTypeName Type { get; }
    
    /// <summary>
    /// If true, this is a global that can be considered pre-declared and should not
    /// have a declaration created when generating C code.
    /// </summary>
    public bool IsPredeclared { get; set; }
    
    /// <summary>
    /// The header this global will be declared in. If `null`, this is a type that does
    /// not require a header reference.
    /// </summary>
    public HeaderName? Header { get; set; }
   
    /// <summary>
    /// The file that contains the global "implementation" (e.g. non-extern declaration)
    /// </summary>
    public CSourceFileName? SourceFileName { get; set; }
   
    /// <summary>
    /// What name this type will have in C
    /// </summary>
    public CGlobalName NameInC { get; set; }
   
    /// <summary>
    /// An expression to set the initial value to
    /// </summary>
    public CBaseExpression? InitialValue { get; set; }
    
    /// <summary>
    /// The optional attribute that should be present on the transpiled global's declaration
    /// </summary>
    public string? AttributeText { get; set; }

    internal GlobalConversionInfo(DefinedGlobal global)
    {
        IlName = global.IlName;
        Type = global.IlType;

        switch (global)
        {
            case DotNetDefinedGlobal dotNetDefinedGlobal:
                SetupDotNetGlobal(dotNetDefinedGlobal);
                break;
            
            case NativeDefinedGlobal nativeGlobal:
                SetupNativeGlobal(nativeGlobal);
                break;
            
            default:
                throw new NotSupportedException(global.GetType().FullName);
        }
    }

    private void SetupDotNetGlobal(DotNetDefinedGlobal global)
    {
        IsPredeclared = false;
        
        var fieldName = $"{global.Definition.DeclaringType.FullName}_{global.Definition.Name}";
        NameInC = new CGlobalName(Utils.MakeValidCName(fieldName));
        
        var declaringNamespace = new IlNamespace(global.Definition.DeclaringType.Namespace);
        Header = Utils.GetHeaderName(declaringNamespace);
        SourceFileName = Utils.GetSourceFileName(declaringNamespace);
    }

    private void SetupNativeGlobal(NativeDefinedGlobal global)
    {
        IsPredeclared = true;
        Header = global.HeaderFile;
        SourceFileName = null;
        NameInC = global.NativeName;
    }
}