using Dntc.Common.Conversion.Planning;
using Dntc.Common.Definitions;

namespace Dntc.Common.Conversion;

public class FileGenerator
{
    private readonly CodeGenerator _codeGenerator;
    private readonly DefinitionCatalog _definitionCatalog;

    public FileGenerator(DefinitionCatalog definitionCatalog, ConversionCatalog conversionCatalog)
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

        await WriteReferencedHeaders(plannedHeader.ReferencedHeaders, writer);

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

            await _codeGenerator.GenerateStructAsync(dotNetType, writer);
            await writer.WriteLineAsync();
        }

        foreach (var method in plannedHeader.DeclaredMethods)
        {
            var dotNetMethod = GetDotNetDefinition(plannedHeader.Name.Value, method);

            await _codeGenerator.GenerateMethodDeclarationAsync(dotNetMethod, writer);
            await writer.WriteLineAsync();
        }

        await writer.WriteLineAsync($"#endif // {guardName}");
    }

    public async Task WriteSourceFileAsync(PlannedSourceFile sourceFile, StreamWriter writer)
    {
        await WriteReferencedHeaders(sourceFile.ReferencedHeaders, writer);

        foreach (var method in sourceFile.ImplementedMethods)
        {
            var definition = GetDotNetDefinition(sourceFile.Name.Value, method);
            await _codeGenerator.GenerateMethodImplementationAsync(definition, writer);
        }
    }

    private static async Task WriteReferencedHeaders(IReadOnlyList<HeaderName> headers, StreamWriter writer)
    {
        foreach (var referencedHeader in headers)
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
    }

    private DotNetDefinedMethod GetDotNetDefinition(string fileName, MethodConversionInfo method)
    {
        var definition = _definitionCatalog.Find(method.MethodId);
        if (definition == null)
        {
            var message = $"No definition found for method `{method.MethodId.Value}` in {fileName}.";
            throw new InvalidOperationException(message);
        }

        if (definition is not DotNetDefinedMethod dotNetMethod)
        {
            var message = $"File '{fileName}' declares method '{method.MethodId.Value}', which " +
                          $"is not a dot net method, but instead is a {definition.GetType().FullName}";
            throw new InvalidOperationException(message);
        }

        return dotNetMethod;
    }
}