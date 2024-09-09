using Dntc.Common.Definitions;

namespace Dntc.Common.Conversion;

public class CodeGenerator
{
    private readonly ConversionCatalog _conversionCatalog;

    public CodeGenerator(ConversionCatalog catalog)
    {
        _conversionCatalog = catalog;
    }
    
    public async Task GenerateStructAsync(DotNetDefinedType type, StreamWriter writer)
    {
        var conversionInfo = _conversionCatalog.Find(type.IlName);
        
        await writer.WriteLineAsync("typedef struct {");
        foreach (var field in type.Fields)
        {
            var fieldConversionInfo = _conversionCatalog.Find(field.Type);
            await writer.WriteLineAsync($"\t{fieldConversionInfo.NameInC.Value} {field.Name};");
        }
        
        await writer.WriteLineAsync($"}} {conversionInfo.NameInC.Value};");
    }

    public async Task GenerateMethodDeclarationAsync(DotNetDefinedMethod method, StreamWriter writer)
    {
        var methodInfo = _conversionCatalog.Find(method.Id);
        var returnTypeInfo = _conversionCatalog.Find(method.ReturnType);

        await writer.WriteAsync($"{returnTypeInfo.NameInC.Value} {methodInfo.NameInC.Value}(");
        for (var x = 0; x < method.Parameters.Count; x++)
        {
            var param = method.Parameters[x];
            var prefix = x > 0 ? ", " : "";
            var paramInfo = _conversionCatalog.Find(param.Type);
            await writer.WriteAsync($"{prefix}{paramInfo.NameInC.Value} {param.Name}");
        }

        await writer.WriteLineAsync(");");
    }

    public async Task GenerateMethodImplementationAsync(DotNetDefinedMethod method, StreamWriter writer)
    {
        var methodInfo = _conversionCatalog.Find(method.Id);
        var returnTypeInfo = _conversionCatalog.Find(method.ReturnType);

        await writer.WriteAsync($"{returnTypeInfo.NameInC.Value} {methodInfo.NameInC.Value}(");
        for (var x = 0; x < method.Parameters.Count; x++)
        {
            var param = method.Parameters[x];
            var prefix = x > 0 ? ", " : "";
            var paramInfo = _conversionCatalog.Find(param.Type);
            await writer.WriteAsync($"{prefix}{paramInfo.NameInC.Value} {param.Name}");
        }

        await writer.WriteLineAsync(") {");

        await writer.WriteLineAsync("\t// TODO....");

        await writer.WriteLineAsync("}");
    }
}