using Dntc.Common.Conversion;

namespace Dntc.Common.Syntax;

public record FieldDeclaration(FieldConversionInfo Field, FieldDeclaration.FieldFlags Flags)
{
    [Flags]
    public enum FieldFlags
    {
        None = 0,
        IsHeaderDeclaration = 1 << 0,
        IgnoreValueInitialization = 1 << 1,
    }
    
    public async Task WriteAsync(StreamWriter writer)
    {
        if (Flags.HasFlag(FieldFlags.IsHeaderDeclaration))
        {
            await writer.WriteAsync("extern ");
        }

        if (Field.IsNonPointerString)
        {
            await writer.WriteAsync($"char {Field.NameInC}[]");
        }
        else
        {
            await writer.WriteAsync($"{Field.FieldTypeConversionInfo.NameInC} {Field.NameInC}");

            if (Field.StaticItemSize != null)
            {
                await writer.WriteAsync($"[{Field.StaticItemSize}]");
            }
        }

        if (!Flags.HasFlag(FieldFlags.IsHeaderDeclaration))
        {
            if (Field.AttributeText != null)
            {
                await writer.WriteAsync($" {Field.AttributeText}");
            }

            if (!Flags.HasFlag(FieldFlags.IgnoreValueInitialization))
            {
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
        }

        await writer.WriteLineAsync(";");
    }
}