using Dntc.Common.Conversion;
using Dntc.Common.Definitions;

namespace Dntc.Common.Syntax;

public record GlobalVariableDeclaration(
    FieldConversionInfo Field, 
    TypeConversionInfo Type,
    bool IsHeaderDeclaration)
{
    public async Task WriteAsync(StreamWriter writer)
    {
        if (IsHeaderDeclaration)
        {
            await writer.WriteAsync("extern ");
        }

        if (Field.IsNonPointerString)
        {
            await writer.WriteAsync($"char {Field.NameInC}[]");
        }
        else
        {
            await writer.WriteAsync($"{Type.NameInC} {Field.NameInC}");
        }

        if (!IsHeaderDeclaration)
        {
            if (Field.AttributeText != null)
            {
                await writer.WriteAsync($" {Field.AttributeText}");
            }
            
            await writer.WriteAsync(" = ");
            if (Field.InitialValue != null)
            {
                await Field.InitialValue.WriteCodeStringAsync(writer);
            }
            else
            {
                await writer.WriteAsync("{0}");
            }
        }

        await writer.WriteLineAsync(";");
    }
}