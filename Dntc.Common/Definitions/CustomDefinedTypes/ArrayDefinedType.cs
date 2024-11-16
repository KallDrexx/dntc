using Dntc.Common.Conversion;
using Dntc.Common.Syntax.Statements;
using Mono.Cecil;

namespace Dntc.Common.Definitions.CustomDefinedTypes;

public class ArrayDefinedType : CustomDefinedType
{
    private readonly TypeReference _arrayType;
    
    public ArrayDefinedType(TypeReference arrayType) 
        : base(
            new IlTypeName(arrayType.FullName), 
            new HeaderName("dotnet_arrays.h"), 
            null, 
            FormNativeName(arrayType),
            [new IlTypeName(arrayType.GetElementType().FullName)],
            [new HeaderName("<stdio.h>"), new HeaderName("<stdlib.h>")])
    {
        if (!arrayType.IsArray)
        {
            var message = $"Expected array type, instead got '{arrayType.FullName}'";
            throw new InvalidOperationException(message);
        }

        _arrayType = arrayType;
        ManuallyReferencedHeaders = [new HeaderName("<stddef.h>")];
    }

    public override CustomCodeStatementSet? GetCustomTypeDeclaration(ConversionCatalog catalog)
    {
        var elementInfo = catalog.Find(new IlTypeName(_arrayType.GetElementType().FullName));

        var content = $@"
typedef struct {{
    size_t length;
    {elementInfo.NameInC} *items;
}} {NativeName};";

        return new CustomCodeStatementSet(content);
    }
    
    private static CTypeName FormNativeName(TypeReference type)
    {
        var elementType = type.GetElementType();
        var convertedName = elementType.FullName
            .Replace(".", "")
            .Replace("/", "");
        
        return new CTypeName($"{convertedName}Array");
    }
}