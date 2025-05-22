using System.Text;
using Dntc.Common.Conversion;
using Dntc.Common.Syntax.Statements;

namespace Dntc.Common.Definitions.CustomDefinedMethods;

public class StaticConstructorInitializerDefinedMethod : CustomDefinedMethod
{
    private const string NamespaceName = "Dntc.Utils";
    private const string MethodName = "InitStaticConstructors";
    private const string BaseFileName = "dntc_utils";
    private const string FunctionName = "dntc_utils_init_static_constructors";

    private readonly HashSet<HeaderName> _referencedHeaders = [];

    public static IlMethodId MethodId = new($"{NamespaceName}.{MethodName}()");
    
    private readonly List<MethodConversionInfo> _staticConstructors = [];
    
    public StaticConstructorInitializerDefinedMethod() 
        : base(
            MethodId,
            new IlTypeName(typeof(void).FullName!), 
            new IlNamespace(NamespaceName), 
            new HeaderName($"{BaseFileName}.h"), 
            new CSourceFileName($"{BaseFileName}.c"), 
            new CFunctionName(FunctionName), 
            [])
    {
    }
    
    public override CustomCodeStatementSet GetCustomDeclaration(ConversionCatalog catalog)
    {
        return new CustomCodeStatementSet($"void {FunctionName}(void)");
    }

    public override CustomCodeStatementSet GetCustomImplementation(ConversionCatalog catalog)
    {
        var content = new StringBuilder();
        foreach (var constructor in _staticConstructors)
        {
            content.AppendLine($"\t{constructor.NameInC}();");
        }
        
        return new CustomCodeStatementSet(content.ToString());
    }

    public void AddStaticConstructor(MethodConversionInfo method)
    {
        _staticConstructors.Add(method);
        if (method.Header != null)
        {
            _referencedHeaders.Add(method.Header.Value);
            ReferencedHeaders = _referencedHeaders.OrderBy(x => x.Value).ToArray();
        }
    }
}