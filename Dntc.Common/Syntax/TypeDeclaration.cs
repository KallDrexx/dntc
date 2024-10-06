using Dntc.Common.Conversion;

namespace Dntc.Common.Syntax;

public record TypeDeclaration(TypeConversionInfo Type)
{
    public async Task WriteAsync(StreamWriter writer)
    {
        await writer.WriteLineAsync("typedef struct {");
        foreach (var field in Type.Fields)
        {
            await writer.WriteLineAsync($"\t{field.Type.NameInC} {field.Name};");
        }

        await writer.WriteLineAsync($"}} {Type.NameInC};");
    }
}
