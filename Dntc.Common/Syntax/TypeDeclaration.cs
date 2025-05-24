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
        // We have to make sure that the type declaration doesn't include the pointer in the name
        var nativeName = TypeConversion.IsReferenceType
            ? TypeConversion.NameInC.Value
            : TypeConversion.NativeNameWithPossiblePointer();

        await writer.WriteLineAsync($"typedef struct {nativeName} {{");

        // Add the entry for the base class. This must be the first entry for pointer casting to work.
        if (dotNetDefinedType.Definition.BaseType != null)
        {
            if (dotNetDefinedType.Definition.BaseType.FullName != typeof(Object).FullName)
            {
                if (Catalog.TryFind(new IlTypeName(dotNetDefinedType.Definition.BaseType.FullName), out var baseType))
                {
                    await writer.WriteLineAsync($"\t{baseType.NameInC} base;");
                }
            }
        }

        // Write all the fields.
        foreach (var field in dotNetDefinedType.InstanceFields)
        {
            var declaration = new FieldDeclaration(
                Catalog.Find(field.Id),
                FieldDeclaration.FieldFlags.IgnoreValueInitialization);

            await writer.WriteAsync("\t");
            await declaration.WriteAsync(writer);
        }

        // Write the virtual table for virtual methods.
        var virtualMethodCount = 0;
        if (!dotNetDefinedType.Definition.IsValueType)
        {
            foreach (var virtualMethod in dotNetDefinedType.Definition.Methods.Where(x => x.IsVirtual && x.IsNewSlot))
            {
                virtualMethodCount++;
                var methodInfo = Catalog.Find(new IlMethodId(virtualMethod.FullName));
                await writer.WriteAsync(
                    $"\t{methodInfo.ReturnTypeInfo.NativeNameWithPossiblePointer()} (*{methodInfo.NameInC})(");

                for (var x = 0; x < methodInfo.Parameters.Count; x++)
                {
                    if (x > 0) await writer.WriteAsync(", ");
                    var param = methodInfo.Parameters[x];
                    var paramInfo = Catalog.Find(param.TypeName);

                    string structKeyword = "";
                    if (x == 0)
                    {
                        structKeyword = "struct ";
                    }

                    var pointerSymbol = param.IsReference ? "*" : "";
                    await writer.WriteAsync($"{structKeyword}{paramInfo.NameInC}{pointerSymbol} {param.Name}");
                }

                await writer.WriteLineAsync(");");
            }
        }

        if (dotNetDefinedType.InstanceFields.Count == 0 && virtualMethodCount == 0)
        {
            // C doesn't allow empty structs
            await writer.WriteLineAsync("\tchar __dummy; // Placeholder for empty type");
        }

        await writer.WriteLineAsync($"}} {nativeName};");
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

        await writer.WriteAsync($"typedef {returnType.NativeNameWithPossiblePointer()} (*{TypeConversion.NativeNameWithPossiblePointer()})(");
        for (var x = 0; x < fnPointer.Definition.Parameters.Count; x++)
        {
            var param = fnPointer.Definition.Parameters[x];
            var paramType = Catalog.Find(new IlTypeName(param.ParameterType.FullName));

            if (x > 0)
            {
                await writer.WriteAsync(", ");
            }

            await writer.WriteAsync(paramType.NativeNameWithPossiblePointer());
        }

        await writer.WriteLineAsync(");");
    }
}