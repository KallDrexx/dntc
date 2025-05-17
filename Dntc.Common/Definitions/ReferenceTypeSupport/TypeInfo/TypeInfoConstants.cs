namespace Dntc.Common.Definitions.ReferenceTypeSupport.TypeInfo;

public static class TypeInfoConstants
{
    public static IlTypeName TypeInfoTypeId = new($"{ReferenceTypeConstants.IlNamespace}.TypeInfo");
    
    public static IlFieldId TypeInfoFieldId
        = new($"{TypeInfoTypeId} {TypeInfoTypeId}::TypeInfo");

    public static CFieldName TypeInfoFieldName = new("type_info");
}