using Dntc.Common.Conversion;

namespace Dntc.Common.Syntax;

public record MethodDeclaration(MethodConversionInfo Method)
{
    public async Task WriteAsync(StreamWriter writer)
    {
        await writer.WriteAsync($"{Method.ReturnTypeInfo.NameInC} {Method.NameInC}(");
        for (var x = 0; x < Method.Parameters.Count; x++)
        {
            if (x > 0) await writer.WriteAsync(", ");

            var param = Method.Parameters[x];
            var pointerSymbol = param.IsReference ? "*" : "";
            await writer.WriteAsync($"{param.ConversionInfo.NameInC} {pointerSymbol}{param.Name}");
        }

        await writer.WriteAsync(")");
    }
}
