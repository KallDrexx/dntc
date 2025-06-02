using System.Text;
using Dntc.Attributes;
using Dntc.Common.Definitions;

namespace Dntc.Common.Conversion;

/// <summary>
/// Contains information of how a type will be converted to C.
/// </summary>
public class TypeConversionInfo
{
    /// <summary>
    /// The name this type has when referenced in .net
    /// </summary>
    public IlTypeName IlName { get; }
    
    /// <summary>
    /// If true, this is a type that can be considered pre-declared and should not
    /// have a declaration created when generating C code.
    /// </summary>
    public bool IsPredeclared { get; set; }
    
    /// <summary>
    /// The header this type will be declared in. If `null`, this is a type that does
    /// not require a header reference (mostly for native types, like `float`), or
    /// it's defined in a source file instead of a header.
    /// </summary>
    public HeaderName? Header { get; set; }
   
    /// <summary>
    /// The source file this type will be declared in. If `null`, this type is not
    /// declared in a source file and is most likely declared in a header file instead.
    /// </summary>
    public CSourceFileName? SourceFileName { get; set; }
   
    /// <summary>
    /// What name this type will have in C
    /// </summary>
    public CTypeName NameInC { get; set; }

    /// <summary>
    /// Headers that need to be referenced along with this type's declaration that can't be
    /// automatically determined.
    /// </summary>
    public IReadOnlyList<HeaderName> ReferencedHeaders { get; private set; } = Array.Empty<HeaderName>();
   
    /// <summary>
    /// The dntc type definition that this conversion info was created from.
    /// </summary>
    public DefinedType OriginalTypeDefinition { get; }
   
    /// <summary>
    /// Designates if this type is a pointer variation of a type or not.
    /// </summary>
    public bool IsPointer { get; private set; }

    /// <summary>
    /// Designates if this type is a reference type or not
    /// </summary>
    public bool IsReferenceType { get; private set; }

    public TypeConversionInfo(DefinedType type, bool isPointer)
    {
        OriginalTypeDefinition = type;
        IlName = type.IlName;
        IsPointer = isPointer;
        
        switch (type)
        {
            case NativeDefinedType nativeDefinedType:
                SetupNativeType(nativeDefinedType);
                break;
            
            case DotNetDefinedType dotNetDefinedType:
                SetupDotNetType(dotNetDefinedType);
                break;
            
            case DotNetFunctionPointerType dotNetFunctionPointerType:
                SetupDotNetFunctionPointer(dotNetFunctionPointerType);
                break;
            
            case CustomDefinedType customDefinedType:
                SetupCustomType(customDefinedType);
                break;
            
            default:
                throw new NotSupportedException(type.GetType().FullName);
        }
    }
    
    private void SetupDotNetType(DotNetDefinedType type)
    {
        IsPredeclared = false;
        NameInC = new CTypeName(Utils.MakeValidCName(type.IlName.Value));
        Header = Utils.GetHeaderName(type.Namespace);
        SourceFileName = null;

        if (!type.Definition.IsValueType)
        {
            IsReferenceType = true;
            ReferencedHeaders = [new HeaderName("<stdlib.h>"), new HeaderName("<string.h>")];
        }
    }

    private void SetupDotNetFunctionPointer(DotNetFunctionPointerType functionPointer)
    {
        IsPredeclared = false;
        Header = new HeaderName("fn_pointer_types.h"); // Have centralized fn pointer declarations
        SourceFileName = null;
        
        // Using shortnames instead of full names risks collisions, but I think it's unlikely,
        // and worth doing until it becomes a problem, as they will be extremely bloated. 
        // To not have bloated names we'd have to maintain a count somewhere, but that also
        // risks names changing between code generation runs non-deterministically.
        var cName = new StringBuilder("FnPtr");
        foreach (var param in functionPointer.Definition.Parameters)
        {
            var paramName = Utils.MakeValidCName(param.ParameterType.Name);
            cName.Append($"_{paramName}");
        }

        var returnName = Utils.MakeValidCName(functionPointer.Definition.ReturnType.Name);
        cName.Append($"_Returns_{returnName}");

        NameInC = new CTypeName(cName.ToString());
    }

    private void SetupNativeType(NativeDefinedType type)
    {
        IsPredeclared = true;
        Header = type.HeaderFile;
        NameInC = type.NativeName;
        SourceFileName = null;
    }

    private void SetupCustomType(CustomDefinedType type)
    {
        IsPredeclared = false;
        Header = type.HeaderName;
        SourceFileName = null;
        NameInC = type.NativeName;
        ReferencedHeaders = type.ReferencedHeaders;
        IsReferenceType = type.IsConsideredDotNetReferenceType;
    }
}