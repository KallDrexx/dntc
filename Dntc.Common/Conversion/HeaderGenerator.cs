using Dntc.Common.Conversion.Planning;
using Dntc.Common.Definitions;

namespace Dntc.Common.Conversion;

public class HeaderGenerator
{
    private readonly CodeGenerator _codeGenerator;
    private readonly DefinitionCatalog _definitionCatalog;

    public HeaderGenerator(DefinitionCatalog definitionCatalog, ConversionCatalog conversionCatalog)
    {
        _codeGenerator = new CodeGenerator(conversionCatalog);
        _definitionCatalog = definitionCatalog;
    }

    public async Task WriteHeaderFileAsync(PlannedHeaderFile plannedHeader, StreamWriter writer)
    {
        var guardName = plannedHeader.Name.Value.Replace(".", "_").ToUpper();
        await writer.WriteLineAsync($"#ifndef {guardName}");
        await writer.WriteLineAsync($"#define {guardName}");
        await writer.WriteLineAsync();
        
        foreach (var referencedHeader in plannedHeader.ReferencedHeaders)
        {
            if (referencedHeader.Value.StartsWith('<'))
            {
                await writer.WriteLineAsync($"#include {referencedHeader.Value}");
            }
            else
            {
                await writer.WriteLineAsync($"#include \"{referencedHeader.Value}\"");
            }
        }

        await writer.WriteLineAsync("");

        foreach (var type in plannedHeader.DeclaredTypes)
        {
            var definition = _definitionCatalog.Find(type.IlName);
            if (definition == null)
            {
                var message = $"No definition found for type `{type.IlName.Value}` in " +
                              $"header '{plannedHeader.Name.Value}'";
                throw new InvalidOperationException(message);
            }

            if (definition is not DotNetDefinedType dotNetType)
            {
                var message = $"Header '{plannedHeader.Name.Value}' declares type '{type.IlName.Value}', which " +
                              $"is not a dot net type, but instead is a {definition.GetType().FullName}";
                throw new InvalidOperationException(message);
            }

            await _codeGenerator.GenerateStruct(dotNetType, writer);
            await writer.WriteLineAsync();
        }

        foreach (var method in plannedHeader.DeclaredMethods)
        {
            var definition = _definitionCatalog.Find(method.MethodId);
            if (definition == null)
            {
                var message = $"No definition found for method `{method.MethodId.Value}` in " +
                              $"header '{plannedHeader.Name.Value}'";
                throw new InvalidOperationException(message);
            }

            if (definition is not DotNetDefinedMethod dotNetMethod)
            {
                var message = $"Header '{plannedHeader.Name.Value}' declares method '{method.MethodId.Value}', which " +
                              $"is not a dot net method, but instead is a {definition.GetType().FullName}";
                throw new InvalidOperationException(message);
            }

            await _codeGenerator.GenerateMethodDeclaration(dotNetMethod, writer);
            await writer.WriteLineAsync();
        }

        await writer.WriteLineAsync($"#endif // {guardName}");
    }
}