using Dntc.Common.Conversion;
using Dntc.Common.Syntax.Expressions;
using Dntc.Common.Syntax.Statements;
using Mono.Cecil;

namespace Dntc.Common.Definitions.CustomDefinedTypes;

public class StaticallySizedArrayDefinedType : ArrayDefinedType
{
    private readonly int _size;
    private readonly IlTypeName _sizeType;

    public StaticallySizedArrayDefinedType(
        TypeReference arrayType,
        IlTypeName ilTypeName,
        int size, 
        IlTypeName sizeType)
        : base(
            arrayType.GetElementType(),
            ilTypeName,
            null,
            null,
            [])
    {
        if (!arrayType.IsArray)
        {
            var message = $"Expected array type, instead got '{arrayType.FullName}'";
            throw new InvalidOperationException(message);
        }

        _size = size;
        _sizeType = sizeType;
    }

    public override CustomCodeStatementSet? GetCustomTypeDeclaration(ConversionCatalog catalog)
    {
        // No direct type needs to be defined, as this relies on the element type to be 
        // defined.
        return null;
    }

    public override CBaseExpression GetArraySizeExpression(CBaseExpression expressionToArray,
        ConversionCatalog conversionCatalog)
    {
        var sizeTypeInfo = conversionCatalog.Find(_sizeType);
        return new LiteralValueExpression(_size.ToString(), sizeTypeInfo);
    }

    public override CBaseExpression GetItemsAccessorExpression(
        CBaseExpression expressionToArray, 
        ConversionCatalog conversionCatalog)
    {
        // The array index can go against the array itself
        return expressionToArray;
    }

    public override CStatementSet GetLengthCheckExpression(
        CBaseExpression arrayLengthField,
        CBaseExpression arrayInstance,
        DereferencedValueExpression index)
    {
        return new ArrayLengthCheckStatementSet(arrayLengthField, arrayInstance, index);
    }
}