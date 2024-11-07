using System.Text;
using Dntc.Common.Conversion;
using Dntc.Common.Syntax.Statements;

namespace Dntc.Common.Definitions.CustomDefinedMethods;

public class StaticConstructorInitializerDefinedMethod : CustomDefinedMethod
{
    private const string Namespace = "Dntc.Utils";
    private const string MethodName = "InitStaticConstructors";
    private const string BaseFileName = "dntc_utils";
    private const string FunctionName = "dntc_utils_init_static_constructors";

    public static IlMethodId MethodId = new($"{Namespace}.{MethodName}()");
    
    private readonly List<MethodConversionInfo> _staticConstructors = [];
    
    public StaticConstructorInitializerDefinedMethod() 
        : base(
            MethodId,
            new IlTypeName(typeof(void).FullName!), 
            new IlNamespace(Namespace), 
            new HeaderName($"{BaseFileName}.h"), 
            new CSourceFileName($"{BaseFileName}.c"), 
            new CFunctionName(FunctionName), 
            [])
    {
    }
    
    public override CustomCodeStatementSet GetCustomDeclaration()
    {
        var headers = _staticConstructors.Select(x => x.Header)
            .Where(x => x != null)
            .Distinct()
            .OrderBy(x => x!.Value)
            .ToArray();

        var content = new StringBuilder();
        foreach (var header in headers)
        {
            content.AppendLine($"#include \"{header}\"");
        }

        content.AppendLine();
        content.AppendLine($"void {FunctionName}(void);");

        return new CustomCodeStatementSet(content.ToString());
    }

    public override CustomCodeStatementSet GetCustomImplementation()
    {
        var content = new StringBuilder();
        content.AppendLine($"void {FunctionName}(void) {{");

        foreach (var constructor in _staticConstructors)
        {
            content.AppendLine($"\t{constructor.NameInC}();");
        }
        
        content.AppendLine("}");

        return new CustomCodeStatementSet(content.ToString());
    }

    public void AddStaticConstructor(MethodConversionInfo method)
    {
        _staticConstructors.Add(method);
    }
}