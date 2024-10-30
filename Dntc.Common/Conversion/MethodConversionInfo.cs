using Dntc.Attributes;
using Dntc.Common.Definitions;
using Dntc.Common.Syntax.Statements;
using Mono.Cecil;

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
    public HeaderName? Header { get; private set; }
   
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

    private void SetupDotNetMethod(DotNetDefinedMethod method)
    {
        var nativeTranspileAttribute = method.Definition
            .CustomAttributes
            .FirstOrDefault(x => x.AttributeType.FullName == typeof(NativeFunctionCallOnTranspileAttribute).FullName);

        if (nativeTranspileAttribute != null)
        {
            SetupNativeOnTranspiledMethod(method, nativeTranspileAttribute);
            return;
        }
        
        IsPredeclared = false;

        Header = Utils.GetHeaderName(method.Namespace);
        SourceFileName = Utils.GetSourceFileName(method.Namespace);

        // TOOD: Need to figure out a good way to disambiguate overloaded functions
        var functionName = $"{method.Definition.DeclaringType.FullName}.{method.Definition.Name}";
        if (method.GenericArgumentTypes.Any())
        {
            var argTypeNames = method.GenericArgumentTypes
                .Select(x => x.Value.Value)
                .Aggregate((x, y) => $"{x}_{y}");

            functionName += $"_{argTypeNames}";
        }
        
        NameInC = new CFunctionName(Utils.MakeValidCName(functionName));
    }

    private void SetupNativeOnTranspiledMethod(DotNetDefinedMethod method, CustomAttribute nativeTranspileAttribute)
    {
        var arguments = nativeTranspileAttribute.ConstructorArguments.ToArray();
        if (arguments.Length != 2)
        {
            var message = $"Method '{method.Id}' is marked as a native call on transpile, but the attribute had " +
                          $"{arguments.Length} arguments when 2 were expected";
            throw new InvalidOperationException(message);
        }

        if (arguments[0].Value?.GetType() != typeof(string))
        {
            var message = $"Method '{method.Id}' is marked as a native call on transpile, but the first attribute " +
                          $"had a value type of '{arguments[0].Value?.GetType().FullName}', but only non-null " +
                          $"strings are allowed.";

            throw new InvalidOperationException(message);

        }

        if (arguments[1].Value != null && arguments[1].Value?.GetType() != typeof(string))
        {
            var message = $"Method '{method.Id}' is marked as a native call on transpile, but the second " +
                          $"attribute had a value type of '{arguments[0].Value?.GetType().FullName}', but " +
                          $"only null or strings are allowed.";

            throw new InvalidOperationException(message);
        }

        IsPredeclared = true;
        NameInC = new CFunctionName(arguments[0].Value.ToString()!);
        Header = arguments[1].Value != null ? new HeaderName(arguments[1].Value.ToString()!) : null;
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