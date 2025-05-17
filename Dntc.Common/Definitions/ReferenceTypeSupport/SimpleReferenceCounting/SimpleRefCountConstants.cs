namespace Dntc.Common.Definitions.ReferenceTypeSupport.SimpleReferenceCounting;

public static class SimpleRefCountConstants
{
    public static IlFieldId CurrentCountFieldId
        = new($"{typeof(int).FullName} {ReferenceTypeConstants.ReferenceTypeBaseId}::ActiveReferenceCount");

    public static CFieldName CurrentCountFieldName = new("activeReferenceCount");
}