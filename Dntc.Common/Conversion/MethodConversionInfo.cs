using Dntc.Common.Definitions;
using Dntc.Common.Syntax.Statements;

namespace Dntc.Common.Conversion;

/// <summary>
/// Contains information on how a method will be converted into a C function.
/// </summary>
public class MethodConversionInfo
{
    public record Parameter(TypeConversionInfo ConversionInfo, string Name, bool IsReference);

    public record Local(TypeConversionInfo ConversionInfo, bool IsReference);
    
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
   
    /// <summary>
    /// Type conversion information for the type this method returns.
    /// </summary>
    public TypeConversionInfo ReturnTypeInfo { get; private set; }
   
    /// <summary>
    /// Conversion info for all parameters this method is defined with.
    /// </summary>
    public IReadOnlyList<Parameter> Parameters { get; private set; }
   
    /// <summary>
    /// Conversion info for all locals used
    /// </summary>
    public IReadOnlyList<Local> Locals { get; private set; }
   
    public MethodConversionInfo(DefinedMethod method, ConversionCatalog conversionCatalog)
    {
        MethodId = method.Id;
        ReturnTypeInfo = conversionCatalog.Find(method.ReturnType);
        Parameters = method.Parameters
            .Select(x => new Parameter(conversionCatalog.Find(x.Type), x.Name, x.IsReference))
            .ToArray();

        Locals = method.Locals
            .Select(x => new Local(conversionCatalog.Find(x.Type), x.IsReference))
            .ToArray();
        
        switch (method)
        {
            case DotNetDefinedMethod dotNetDefinedMethod:
                SetupDotNetMethod(dotNetDefinedMethod);
                break;
            
            case NativeDefinedMethod nativeDefinedMethod:
                SetupNativeMethod(nativeDefinedMethod);
                break;
            
            case CustomDefinedMethod customDefinedMethod:
                SetupCustomMethod(customDefinedMethod);
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
        if (method.GenericArgumentTypes.Any())
        {
            var argTypeNames = method.GenericArgumentTypes
                .Select(x => x.Value.Value)
                .Aggregate((x, y) => $"{x}_{y}");

            functionName += $"_{argTypeNames}";
        }
        
        NameInC = new CFunctionName(ConvertNameToC(functionName));
    }

    private void SetupNativeMethod(NativeDefinedMethod method)
    {
        IsPredeclared = true;
        Header = method.HeaderFile;
        NameInC = method.NativeName;
    }

    private void SetupCustomMethod(CustomDefinedMethod method)
    {
        IsPredeclared = false; // we need to write the custom code
        Header = method.HeaderName;
        NameInC = method.NativeName;
    }
}