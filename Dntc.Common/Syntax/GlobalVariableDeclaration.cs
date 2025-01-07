using Dntc.Common.Conversion;
using Dntc.Common.Definitions;

namespace Dntc.Common.Syntax;

public record GlobalVariableDeclaration(
    GlobalConversionInfo Global, 
    TypeConversionInfo Type,
    bool IsHeaderDeclaration)
{
    public async Task WriteAsync(StreamWriter writer)
    {
        if (IsHeaderDeclaration)
        {
            await writer.WriteAsync("extern ");
        }

        await writer.WriteAsync($"{Type.NameInC} {Global.NameInC}");

        if (!IsHeaderDeclaration)
        {
            if (Global.AttributeText != null)
            {
                await writer.WriteAsync($" {Global.AttributeText}");
            }
            
            await writer.WriteAsync(" = ");
            if (Global.InitialValue != null)
            {
                await Global.InitialValue.WriteCodeStringAsync(writer);
            }
            else
            {
                await writer.WriteAsync("{0}");
            }
        }

        await writer.WriteLineAsync(";");
    }
}