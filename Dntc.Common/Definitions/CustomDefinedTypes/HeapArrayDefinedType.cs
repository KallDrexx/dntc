using Dntc.Common.Conversion;
using Dntc.Common.Syntax.Expressions;
using Dntc.Common.Syntax.Statements;
using Mono.Cecil;

namespace Dntc.Common.Definitions.CustomDefinedTypes;

public class HeapArrayDefinedType : ArrayDefinedType
{
    private readonly TypeReference _arrayType;
    
    public HeapArrayDefinedType(TypeReference arrayType) 
        : base(
            new IlTypeName(arrayType.GetElementType().FullName),
            new IlTypeName(arrayType.FullName), 
            new HeaderName("dotnet_arrays.h"), 
            null, 
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
    int32_t length;
    {elementInfo.NameInC} *items;
}} {NativeName};";

        return new CustomCodeStatementSet(content);
    }
    
    public override CBaseExpression GetArraySizeExpression(
        CBaseExpression expressionToArray,
        ConversionCatalog conversionCatalog)
    {
        var int32Type = conversionCatalog.Find(new IlTypeName(typeof(int).FullName!));
        return new FieldAccessExpression(expressionToArray, new Variable(int32Type, "length", false));
    }

    public override CBaseExpression GetItemsAccessorExpression(
        CBaseExpression expressionToArray, 
        ConversionCatalog conversionCatalog)
    {
        var elementType = conversionCatalog.Find(ElementType);
        var items = new Variable(elementType, "items", false);
        return new FieldAccessExpression(expressionToArray, items);
    }

    public override CStatementSet GetLengthCheckExpression(
        CBaseExpression arrayLengthField,
        CBaseExpression arrayInstance,
        DereferencedValueExpression index)
    {
        return new ArrayLengthCheckStatementSet(arrayLengthField, arrayInstance, index);
    }

    public override CTypeName FormTypeName(TypeConversionInfo elementTypeInfo)
    {
        var convertedName = Utils.MakeValidCName(ElementType.Value).Replace("_", "");

        return new CTypeName($"{convertedName}Array");
    }
}