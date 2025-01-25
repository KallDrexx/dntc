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

            case DotNetFunctionPointerType functionPointerType:
                await WriteFunctionPointerType(writer, functionPointerType);
                break;

            default:
                throw new NotSupportedException(TypeDefinition.GetType().FullName);
        }
    }

    private async Task WriteDotNetDefinedTypeAsync(StreamWriter writer, DotNetDefinedType dotNetDefinedType)
    {
        await writer.WriteLineAsync("typedef struct {");
        foreach (var field in dotNetDefinedType.InstanceFields)
        {
            var declaration = new FieldDeclaration(
                Catalog.Find(field.Id),
                FieldDeclaration.FieldFlags.IgnoreValueInitialization);

            await writer.WriteAsync("\t");
            await declaration.WriteAsync(writer);
        }

        if (dotNetDefinedType.InstanceFields.Count == 0)
        {
            // C doesn't allow empty structs
            await writer.WriteLineAsync("\tchar __dummy; // Placeholder for empty type");
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

    private async Task WriteFunctionPointerType(StreamWriter writer, DotNetFunctionPointerType fnPointer)
    {
        var returnType = Catalog.Find(new IlTypeName(fnPointer.Definition.ReturnType.FullName));

        await writer.WriteAsync($"typedef {returnType.NameInC} (*{TypeConversion.NameInC})(");
        for (var x = 0; x < fnPointer.Definition.Parameters.Count; x++)
        {
            var param = fnPointer.Definition.Parameters[x];
            var paramType = Catalog.Find(new IlTypeName(param.ParameterType.FullName));

            if (x > 0)
            {
                await writer.WriteAsync(", ");
            }

            await writer.WriteAsync(paramType.NameInC.Value);
        }

        await writer.WriteLineAsync(");");
    }
}