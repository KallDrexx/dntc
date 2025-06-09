using Dntc.Common.Conversion;
using Dntc.Common.Syntax.Expressions;
using Dntc.Common.Syntax.Statements;
using Mono.Cecil;

namespace Dntc.Common.Definitions.CustomDefinedTypes;

public class StaticallySizedArrayDefinedType : ArrayDefinedType
{
    private readonly int _size;
    private readonly IlTypeName _sizeType;
    private readonly bool _bypassBoundsCheck;

    public StaticallySizedArrayDefinedType(
        TypeReference arrayType,
        IlTypeName ilTypeName,
        int size, 
        IlTypeName sizeType,
        bool bypassBoundsCheck)
        : base(
            new IlTypeName(arrayType.GetElementType().FullName),
            ilTypeName,
            null,
            null,
            [new HeaderName("<stdio.h>"), new HeaderName("<stdlib.h>")])
    {
        if (!arrayType.IsArray)
        {
            var message = $"Expected array type, instead got '{arrayType.FullName}'";
            throw new InvalidOperationException(message);
        }

        _size = size;
        _sizeType = sizeType;
        _bypassBoundsCheck = bypassBoundsCheck;
    }

    public StaticallySizedArrayDefinedType(
        IlTypeName arrayElementType,
        IlTypeName ilTypeName,
        int size,
        IlTypeName sizeType,
        bool bypassBoundsCheck)
        : base(
            arrayElementType,
            ilTypeName,
            null,
            null,
            [new HeaderName("<stdio.h>"), new HeaderName("<stdlib.h>")])
    {
        _size = size;
        _sizeType = sizeType;
        _bypassBoundsCheck = bypassBoundsCheck;
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
        return new LiteralValueExpression(_size.ToString(), sizeTypeInfo, 0);
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
        CBaseExpression index)
    {
        return _bypassBoundsCheck
            ? new CustomCodeStatementSet(string.Empty)
            : new ArrayLengthCheckStatementSet(arrayLengthField, arrayInstance, index);
    }

    public override CTypeName FormTypeName(TypeConversionInfo elementTypeInfo)
    {
        return elementTypeInfo.NameInC;
    }
}