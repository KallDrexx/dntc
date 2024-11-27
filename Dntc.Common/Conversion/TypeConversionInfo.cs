using System.Text;
using Dntc.Common.Definitions;
using Dntc.Common.Syntax.Statements;

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
    public bool IsPredeclared { get; private set; }
    
    /// <summary>
    /// The header this type will be declared in. If `null`, this is a type that does
    /// not require a header reference (mostly for native types, like `float`).
    /// </summary>
    public HeaderName? Header { get; private set; }
   
    /// <summary>
    /// What name this type will have in C
    /// </summary>
    public CTypeName NameInC { get; private set; }

    /// <summary>
    /// Headers that need to be referenced along with this type's declaration that can't be
    /// automatically determined.
    /// </summary>
    public IReadOnlyList<HeaderName> ReferencedHeaders { get; private set; } = Array.Empty<HeaderName>();

    public TypeConversionInfo(DefinedType type)
    {
        IlName = type.IlName;
        
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
        Header = Utils.GetHeaderName(type.Namespace);
        NameInC = new CTypeName(Utils.MakeValidCName(type.IlName.Value));
    }

    private void SetupDotNetFunctionPointer(DotNetFunctionPointerType functionPointer)
    {
        IsPredeclared = false;
        Header = new HeaderName("fn_pointer_types.h"); // Have centralized fn pointer declarations
        
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
    }

    private void SetupCustomType(CustomDefinedType type)
    {
        IsPredeclared = false;
        Header = type.HeaderName;
        NameInC = type.NativeName;
        ReferencedHeaders = type.ReferencedHeaders;
    }
}