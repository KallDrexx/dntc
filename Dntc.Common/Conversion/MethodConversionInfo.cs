using Dntc.Common.Definitions;

namespace Dntc.Common.Conversion;

/// <summary>
/// Contains information on how a method will be converted into a C function.
/// </summary>
public class MethodConversionInfo
{
    public IlMethodId MethodId { get; private set; }
    
    /// <summary>
    /// If true, then this is a function that has already been declared in
    /// either the C standard or in C code that the converted IL will be
    /// integrated with. Therefore, if true it should not be declared
    /// in the conversion process.
    /// </summary>
    public bool IsPredeclared { get; private set; }
   
    /// <summary>
    /// The header the function is declared in.
    /// </summary>
    public HeaderName Header { get; private set; }
   
    /// <summary>
    /// The file that the function is implemented in. If null, then it is
    /// either part of the C standard or implemented in the integrated C
    /// code base.
    /// </summary>
    public CSourceFileName? SourceFileName { get; private set; }
    
    /// <summary>
    /// The name of the function when defined in C
    /// </summary>
    public CFunctionName NameInC { get; private set; }

    public MethodConversionInfo(DefinedMethod method)
    {
        MethodId = method.Id;
        switch (method)
        {
            case DotNetDefinedMethod dotNetDefinedMethod:
                SetupDotNetMethod(dotNetDefinedMethod);
                break;
            
            default:
                throw new NotSupportedException(method.GetType().FullName);
        }
    }

    private static string ConvertNameToC(string name)
    {
        return name.Replace(".", "_")
            .Replace("/", "__"); // Instance methods have the type name with a slash in it
    }

    private void SetupDotNetMethod(DotNetDefinedMethod method)
    {
        IsPredeclared = false;

        var fileNameBase = ConvertNameToC(method.Namespace.Value);
        Header = new HeaderName(fileNameBase + ".h");
        SourceFileName = new CSourceFileName(fileNameBase + ".c");

        // TOOD: Need to figure out a good way to disambiguate overloaded functions
        var functionName = $"{method.Definition.DeclaringType.FullName}.{method.Definition.Name}";
        NameInC = new CFunctionName(ConvertNameToC(functionName));
    }
}