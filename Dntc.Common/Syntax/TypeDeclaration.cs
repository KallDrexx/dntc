using Dntc.Common.Conversion;
using Dntc.Common.Definitions;

namespace Dntc.Common.Syntax;

public record TypeDeclaration(TypeConversionInfo TypeConversion, DefinedType TypeDefinition, ConversionCatalog Catalog)
{
    public async Task WriteAsync(StreamWriter writer)
    {
        switch (TypeDefinition)
        {
            case DotNetDefinedType dotNetDefinedType:
                await WriteDotNetDefinedTypeAsync(writer, dotNetDefinedType);
                break;
            
            case NativeDefinedType:
                break; // Nothing to do
            
            case CustomDefinedType customDefinedType:
                await WriteCustomDefinedType(writer, customDefinedType);
                break;
            
            default:
                throw new NotSupportedException(TypeDefinition.GetType().FullName);
        }
    }

    private async Task WriteDotNetDefinedTypeAsync(StreamWriter writer, DotNetDefinedType dotNetDefinedType)
    {
        await writer.WriteLineAsync("typedef struct {");
        foreach (var field in dotNetDefinedType.Fields)
        {
            var fieldType = Catalog.Find(field.Type);
            await writer.WriteLineAsync($"\t{fieldType.NameInC} {field.Name};");
        }

        await writer.WriteLineAsync($"}} {TypeConversion.NameInC};");
    }

    private async Task WriteCustomDefinedType(
        StreamWriter writer,
        CustomDefinedType customDefinedType)
    {
        var customCode = customDefinedType.GetCustomTypeDeclaration(Catalog);
        if (customCode != null)
        {
            await customCode.WriteAsync(writer);
        }
    }
}
