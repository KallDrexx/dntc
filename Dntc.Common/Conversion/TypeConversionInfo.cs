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
            
            default:
                throw new NotSupportedException(type.GetType().FullName);
        }
    }

    private static string ConvertNameToC(string name)
    {
        return name.Replace(".", "_")
            .Replace("/", "_"); // Nested types have a slash in them
    }

    private void SetupDotNetType(DotNetDefinedType type)
    {
        IsPredeclared = false;
        Header = new HeaderName(ConvertNameToC(type.Namespace.Value) + ".h");
        NameInC = new CTypeName(ConvertNameToC(type.IlName.Value));
    }

    private void SetupNativeType(NativeDefinedType type)
    {
        IsPredeclared = true;
        Header = type.HeaderFile;
        NameInC = type.NativeName;
    }
}