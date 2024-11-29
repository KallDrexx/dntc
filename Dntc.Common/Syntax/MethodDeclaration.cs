using Dntc.Common.Conversion;
using Dntc.Common.Definitions;

namespace Dntc.Common.Syntax;

public record MethodDeclaration(MethodConversionInfo Method, DefinedMethod Definition, ConversionCatalog Catalog)
{
    public async Task WriteAsync(StreamWriter writer)
    {
        switch (Definition)
        {
            case DotNetDefinedMethod dotNetDefinedMethod:
                await WriteAsync(writer, dotNetDefinedMethod);
                break;

            case CustomDefinedMethod customDefinedMethod:
                await WriteAsync(writer, customDefinedMethod);
                break;

            case NativeDefinedMethod:
                break; // Predefined method, so nothing to do

            default:
                throw new NotSupportedException(Definition.GetType().FullName);
        }
    }

    private async Task WriteAsync(StreamWriter writer, DotNetDefinedMethod dotNetDefinedMethod)
    {
        if (Method.CustomDeclaration != null)
        {
            await writer.WriteAsync(Method.CustomDeclaration);
        }
        else
        {
            await writer.WriteAsync($"{Method.ReturnTypeInfo.NameInC} {Method.NameInC}(");
            for (var x = 0; x < dotNetDefinedMethod.Parameters.Count; x++)
            {
                if (x > 0) await writer.WriteAsync(", ");

                var param = dotNetDefinedMethod.Parameters[x];
                var paramType = Catalog.Find(param.Type);

                var pointerSymbol = param.IsReference ? "*" : "";
                await writer.WriteAsync($"{paramType.NameInC} {pointerSymbol}{param.Name}");
            }

            await writer.WriteAsync(")");
        }
    }

    private async Task WriteAsync(StreamWriter writer, CustomDefinedMethod customDefinedMethod)
    {
        var customCode = customDefinedMethod.GetCustomDeclaration();
        if (customCode != null)
        {
            await customCode.WriteAsync(writer);
        }
    }
}