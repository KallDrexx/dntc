using Dntc.Common.Conversion;
using Dntc.Common.Syntax.Expressions;
using Mono.Cecil;

namespace Dntc.Common.Definitions.CustomDefinedTypes;

public abstract class ArrayDefinedType : CustomDefinedType
{
    public IlTypeName ElementType { get; }
    
    protected ArrayDefinedType(
        TypeReference elementType,
        IlTypeName ilTypeName, 
        HeaderName headerName, 
        CSourceFileName? sourceFileName, 
        CTypeName nativeName, 
        IReadOnlyList<IlTypeName> otherReferencedTypes, 
        IReadOnlyList<HeaderName> referencedHeaders) 
        : base(ilTypeName, headerName, sourceFileName, nativeName, otherReferencedTypes, referencedHeaders)
    {
        ElementType = new IlTypeName(elementType.FullName);
    }

    /// <summary>
    /// Returns an expression that contains the size of the array
    /// </summary>
    public abstract CBaseExpression GetArraySizeExpression(
        CBaseExpression expressionToArray, 
        ConversionCatalog conversionCatalog);
}