using Dntc.Common.Definitions;
using Dntc.Common.Planning;

namespace Dntc.Common.Conversion;

public class FileGenerator
{
    private readonly CodeGenerator _codeGenerator;
    private readonly DefinitionCatalog _definitionCatalog;

    public FileGenerator(DefinitionCatalog definitionCatalog, ConversionCatalog conversionCatalog)
    {
        _codeGenerator = new CodeGenerator(conversionCatalog, definitionCatalog);
        _definitionCatalog = definitionCatalog;
    }

    public async Task WriteHeaderFileAsync(PlannedHeaderFile plannedHeader, StreamWriter writer)
    {
        var guardName = plannedHeader.Name.Value.Replace(".", "_").ToUpper();
        await writer.WriteLineAsync($"#ifndef {guardName}");
        await writer.WriteLineAsync($"#define {guardName}");
        await writer.WriteLineAsync();

        await WriteReferencedHeaders(plannedHeader.Name, plannedHeader.ReferencedHeaders, writer);

        foreach (var type in plannedHeader.DeclaredTypes)
        {
            var definition = _definitionCatalog.Get(type.IlName);
            if (definition == null)
            {
                var message = $"No definition found for type `{type.IlName.Value}` in " +
                              $"header '{plannedHeader.Name.Value}'";
                throw new InvalidOperationException(message);
            }

            switch (definition)
            {
                case DotNetDefinedType dotNetType:
                    await _codeGenerator.GenerateStructAsync(dotNetType, writer);
                    break;

                case DotNetFunctionPointerType fnPtr:
                    await _codeGenerator.GenerateFunctionPointerTypedef(fnPtr, writer);
                    break;

                case CustomDefinedType customDefinedType:
                    await _codeGenerator.GenerateCustomDefinedHeaderData(customDefinedType, writer);
                    break;

                default:
                    var message = $"Header '{plannedHeader.Name.Value}' declares type '{type.IlName.Value}', which " +
                                  $"is a {definition.GetType().FullName}, which is not supported";
                    throw new NotSupportedException(message);
            }

            await writer.WriteLineAsync();
        }

        foreach (var method in plannedHeader.DeclaredMethods)
        {
            var definition = _definitionCatalog.Get(method.MethodId);
            switch (definition)
            {
                case DotNetDefinedMethod dotNetDefinedMethod:
                    await _codeGenerator.GenerateMethodDeclarationAsync(dotNetDefinedMethod, writer);
                    break;
                
                case CustomDefinedMethod customDefinedMethod:
                    await _codeGenerator.GenerateMethodDeclarationAsync(customDefinedMethod, writer);
                    break;
                
                case null:
                    var message = $"No definition found for method `{method.MethodId.Value}` in {plannedHeader.Name.Value}.";
                    throw new InvalidOperationException(message);
                
                default:
                    throw new NotSupportedException(definition.GetType().FullName);
            }

            await writer.WriteLineAsync();
        }

        await writer.WriteLineAsync($"#endif // {guardName}");
    }

    public async Task WriteSourceFileAsync(PlannedSourceFile sourceFile, StreamWriter writer)
    {
        await WriteReferencedHeaders(null, sourceFile.ReferencedHeaders, writer);

        foreach (var method in sourceFile.ImplementedMethods)
        {
            var definition = _definitionCatalog.Get(method.MethodId);
            switch (definition)
            {
                case DotNetDefinedMethod dotNetDefinedMethod:
                    await _codeGenerator.GenerateMethodImplementationAsync(dotNetDefinedMethod, writer);
                    break;
                
                case CustomDefinedMethod customDefinedMethod:
                    await _codeGenerator.GenerateMethodImplementationAsync(customDefinedMethod, writer);
                    break;
                
                case null:
                    var message = $"No definition found for method `{method.MethodId.Value}` in {sourceFile.Name.Value}.";
                    throw new InvalidOperationException(message);
                
                default:
                    throw new NotSupportedException(definition.GetType().FullName);
            }
            
            await writer.WriteLineAsync();
        }
    }

    private static async Task WriteReferencedHeaders(
        HeaderName? currentHeaderName,
        IReadOnlyList<HeaderName> headers,
        StreamWriter writer)
    {
        foreach (var referencedHeader in headers)
        {
            if (currentHeaderName != null && currentHeaderName == referencedHeader)
            {
                continue;
            }

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
}