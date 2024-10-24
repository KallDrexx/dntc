using Dntc.Common.Conversion;
using Dntc.Common.Definitions;

namespace Dntc.Common.Syntax;

public record GlobalVariableDeclaration(
    DefinedType TypeDefinition, 
    DefinedType.Field Field, 
    ConversionCatalog Catalog,
    bool IsExtern)
{
    public async Task WriteAsync(StreamWriter writer)
    {
        var containerTypeInfo = Catalog.Find(TypeDefinition.IlName);
        var fieldTypeInfo = Catalog.Find(Field.Type);

        await writer.WriteLineAsync(
            $"{(IsExtern ? "extern" : "")} {fieldTypeInfo.NameInC} {Utils.StaticFieldName(containerTypeInfo, Field)};");
    }
}