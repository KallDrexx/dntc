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
    
    private readonly List<MethodConversionInfo> _staticConstructors = [];
    
    public StaticConstructorInitializerDefinedMethod() 
        : base(
            new IlMethodId($"{Namespace}.{MethodName}()"), 
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
        const string content = $"void {FunctionName}(void);";

        return new CustomCodeStatementSet(content);
    }

    public override CustomCodeStatementSet GetCustomImplementation()
    {
        var content = new StringBuilder();
        content.AppendLine($"void {FunctionName}(void) {{");
        
        
    }
}