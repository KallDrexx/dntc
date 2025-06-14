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
            await writer.WriteAsync($"{Method.ReturnTypeInfo.NativeNameWithPossiblePointer()} {Method.NameInC}(");
            if (dotNetDefinedMethod.Parameters.Count == 0)
            {
                await writer.WriteAsync("void");
            }
            
            for (var x = 0; x < dotNetDefinedMethod.Parameters.Count; x++)
            {
                if (x > 0) await writer.WriteAsync(", ");

                var param = dotNetDefinedMethod.Parameters[x];
                var paramType = Catalog.Find(param.Type);

                // Don't prepend the parameter with an asterisk if it's a reference type but parameter's C name
                // already contains an asterisk. This usually causes unintended behavior, especially with instances
                // of strings, where the type is already `char *`.
                var pointerSymbol = " ";
                if (param.IsReference && !paramType.NameInC.Value.EndsWith('*'))
                {
                    // For ref reference types: use double pointer
                    if (param.IsReferenceTypeByRef && paramType.IsReferenceType)
                    {
                        pointerSymbol = " **";
                    }
                    else
                    {
                        pointerSymbol = " *";
                    }
                }

                if (paramType.OriginalTypeDefinition is DotNetFunctionPointerType)
                {
                    pointerSymbol = " ";
                }
                
                await writer.WriteAsync($"{paramType.NameInC}{pointerSymbol}{param.Name}");
            }

            await writer.WriteAsync(")");
        }
    }

    private async Task WriteAsync(StreamWriter writer, CustomDefinedMethod customDefinedMethod)
    {
        var customCode = customDefinedMethod.GetCustomDeclaration(Catalog);
        if (customCode != null)
        {
            await customCode.WriteAsync(writer);
        }
    }
}