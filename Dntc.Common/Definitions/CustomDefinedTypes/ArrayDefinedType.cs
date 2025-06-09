using Dntc.Common.Conversion;
using Dntc.Common.Syntax.Expressions;
using Dntc.Common.Syntax.Statements;
using Mono.Cecil;

namespace Dntc.Common.Definitions.CustomDefinedTypes;

public abstract class ArrayDefinedType : CustomDefinedType
{
    public IlTypeName ElementType { get; }
    
    protected ArrayDefinedType(
        IlTypeName elementType,
        IlTypeName arrayTypeName,
        HeaderName? headerName,
        CSourceFileName? sourceFileName, 
        IReadOnlyList<HeaderName> referencedHeaders)
        : base(arrayTypeName,
            headerName, 
            sourceFileName, 
            new CTypeName("<Late bound array name>"),
            [elementType],
            referencedHeaders)
    {
        ElementType = elementType;
    }

    /// <summary>
    /// Returns an expression that contains the size of the array
    /// </summary>
    public abstract CBaseExpression? GetArraySizeExpression(
        CBaseExpression expressionToArray, 
        ConversionCatalog conversionCatalog);

    /// <summary>
    /// Returns an expression that provides access to the items in the array
    /// </summary>
    public abstract CBaseExpression GetItemsAccessorExpression(
        CBaseExpression expressionToArray, 
        ConversionCatalog conversionCatalog);

    /// <summary>
    /// Returns an expression for checking the length of an array
    /// </summary>
    public abstract CStatementSet GetLengthCheckExpression(
        CBaseExpression arrayLengthField,
        CBaseExpression arrayInstance,
        CBaseExpression index);

    /// <summary>
    /// Returns the C name for the array's element type
    /// </summary>
    /// <param name="elementTypeInfo"></param>
    /// <returns></returns>
    public abstract CTypeName FormTypeName(TypeConversionInfo elementTypeInfo);
}