using Dntc.Attributes;
using Dntc.Common.Definitions;
using Dntc.Common.Syntax.Expressions;

namespace Dntc.Common.Conversion;

/// <summary>
/// Contains information for how a global will be converted to C
/// </summary>
public class FieldConversionInfo
{
    /// <summary>
    /// The name this global has when referenced in .net
    /// </summary>
    public IlFieldId IlName { get; }
   
    /// <summary>
    /// The conversion info for the field's value
    /// </summary>
    public TypeConversionInfo FieldTypeConversionInfo { get; set; }
    
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
    public CFieldName NameInC { get; set; }
   
    /// <summary>
    /// An expression to set the initial value to
    /// </summary>
    public CBaseExpression? InitialValue { get; set; }
    
    /// <summary>
    /// The optional attribute that should be present on the transpiled field's declaration
    /// </summary>
    public string? AttributeText { get; set; }
   
    /// <summary>
    /// If true, this global is a `char name[]` instead of `char* name`
    /// </summary>
    public bool IsNonPointerString { get; set; }
   
    /// <summary>
    /// If a non-null value, this is considered an array of values with a compile time size of
    /// the specified value.
    /// </summary>
    public int? StaticItemSize { get; set; }
    
    internal FieldConversionInfo(DefinedField field, TypeConversionInfo fieldType)
    {
        IlName = field.IlName;
        FieldTypeConversionInfo = fieldType;

        switch (field)
        {
            case DotNetDefinedField dotNetDefinedGlobal:
                SetupDotNetGlobal(dotNetDefinedGlobal);
                break;
            
            case NativeDefinedField nativeGlobal:
                SetupNativeGlobal(nativeGlobal);
                break;
            
            default:
                throw new NotSupportedException(field.GetType().FullName);
        }
    }

    private void SetupDotNetGlobal(DotNetDefinedField field)
    {
        IsPredeclared = false;

        var fieldName = field.IsGlobal
            ? $"{field.Definition.DeclaringType.FullName}_{field.Definition.Name}"
            : field.Definition.Name;
        
        NameInC = new CFieldName(Utils.MakeValidCName(fieldName));

        var declaringNamespace = Utils.GetNamespace(field.Definition.DeclaringType);
        Header = Utils.GetHeaderName(declaringNamespace);
        SourceFileName = Utils.GetSourceFileName(declaringNamespace);
    }

    private void SetupNativeGlobal(NativeDefinedField field)
    {
        IsPredeclared = true;
        Header = field.HeaderFile;
        SourceFileName = null;
        NameInC = field.NativeName;
    }
}