using Dntc.Common.Conversion;
using Dntc.Common.Definitions;
using Mono.Cecil;

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
        await writer.WriteLineAsync($"typedef struct {TypeConversion.NativeNameWithPossiblePointer()} {{");

        if (dotNetDefinedType.Definition.BaseType != null)
        {
            if (dotNetDefinedType.Definition.FullName != dotNetDefinedType.Definition.BaseType.FullName)
            {
                var baseType = Catalog.Find(new IlTypeName(dotNetDefinedType.Definition.BaseType.FullName));

                await writer.WriteLineAsync($"\t{baseType.NativeNameWithPossiblePointer()} base;");

                foreach (var virtualMethod in dotNetDefinedType.Definition.Methods.Where(x => x.IsVirtual && x.IsNewSlot))
                {
                    var methodInfo = Catalog.Find(new IlMethodId(virtualMethod.FullName));
                    await writer.WriteAsync($"\t{methodInfo.ReturnTypeInfo.NativeNameWithPossiblePointer()} (*{methodInfo.NameInC})(");
                    
                    for (var x = 0; x < methodInfo.Parameters.Count; x++)
                    {
                        if (x > 0) await writer.WriteAsync(", ");
                        var param = methodInfo.Parameters[x];
                        var paramType = param.ConversionInfo;

                        string structKeyword = "";
                        if (x == 0)
                        {
                            structKeyword = "struct ";
                        }

                        var pointerSymbol = param.IsReference ? "*" : "";
                        await writer.WriteAsync($"{structKeyword}{paramType.NameInC}{pointerSymbol} {param.Name}");
                    }

                    await writer.WriteLineAsync(");");
                }
            }
        }
        
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

        await writer.WriteLineAsync($"}} {TypeConversion.NativeNameWithPossiblePointer()};");

        if (!dotNetDefinedType.Definition.IsValueType)
        {
            // Add create method.
            /*var returnType = TypeConversion.NativeNameWithPointer();
            var methodName = TypeConversion.NameInC + "__Create";

            await writer.WriteLineAsync();
            await writer.WriteLineAsync($"{returnType} {methodName}(void){{");
            
            await writer.WriteLineAsync($"\t{returnType} result = ({returnType}) malloc(sizeof({TypeConversion.NameInC}));");
            await writer.WriteLineAsync($"\tmemset(result, 0, sizeof({TypeConversion.NameInC}));");
            
            foreach (var virtualMethod in dotNetDefinedType.Definition.Methods.Where(x => x.IsVirtual))
            {
                if (virtualMethod.IsReuseSlot)
                {
                    var sourceMethod = Catalog.Find(new IlMethodId(virtualMethod.FullName));
                    var baseType = virtualMethod.DeclaringType.BaseType.Resolve();
                    
                    var targetMethod = FindMatchingMethodInBaseTypes(sourceMethod, baseType);

                    if (targetMethod != null)
                    {
                        var thisExpression = targetMethod.Parameters[0];

                        await writer.WriteAsync($"\t(({thisExpression.ConversionInfo.NameInC}*)result)->{targetMethod.NameInC} = (");

                        var methodInfo = targetMethod;
                        await writer.WriteAsync($"{methodInfo.ReturnTypeInfo.NativeNameWithPossiblePointer()} (*)(");
                    
                        for (var x = 0; x < methodInfo.Parameters.Count; x++)
                        {
                            if (x > 0) await writer.WriteAsync(", ");
                            var param = methodInfo.Parameters[x];
                            var paramType = param.ConversionInfo;

                            var pointerSymbol = param.IsReference ? "*" : "";
                            await writer.WriteAsync($"{paramType.NameInC}{pointerSymbol}");
                        }
                        
                        await writer.WriteLineAsync($")){sourceMethod.NameInC};");
                    }
                }
                else
                {
                    var methodInfo = Catalog.Find(new IlMethodId(virtualMethod.FullName));

                    await writer.WriteLineAsync($"\tresult->{methodInfo.NameInC} = {methodInfo.NameInC};");
                }
            }
            
            await writer.WriteLineAsync($"\treturn result;");

            await writer.WriteLineAsync("}");*/
        }
    }
    
    private MethodConversionInfo? FindMatchingMethodInBaseTypes(MethodConversionInfo sourceMethod, TypeDefinition startingBaseType)
    {
        var currentBaseType = startingBaseType;
    
        while (currentBaseType != null)
        {
            var type = Catalog.Find(new IlTypeName(currentBaseType.FullName));
        
            foreach (var method in type.OriginalTypeDefinition.Methods)
            {
                var methodInfo = Catalog.Find(method);
            
                if (sourceMethod.Name != methodInfo.Name)
                    continue;
            
                if (sourceMethod.ReturnTypeInfo != methodInfo.ReturnTypeInfo)
                    continue;

                if (sourceMethod.Parameters.Count != methodInfo.Parameters.Count)
                    continue;

                bool parametersMatch = true;
                for (int i = 1; i < sourceMethod.Parameters.Count; i++)
                {
                    if (sourceMethod.Parameters[i].ConversionInfo != 
                        methodInfo.Parameters[i].ConversionInfo)
                    {
                        parametersMatch = false;
                        break;
                    }
                }

                if (parametersMatch)
                {
                    return methodInfo;
                }
            }
        
            // Move to the next base type in the hierarchy
            currentBaseType = currentBaseType.BaseType.Resolve();
        }
    
        // If we've gone through all base types and found nothing
        return null;
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