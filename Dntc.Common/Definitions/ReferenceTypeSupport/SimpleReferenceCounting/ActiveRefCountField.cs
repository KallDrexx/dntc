using Dntc.Common.Syntax.Statements;

namespace Dntc.Common.Definitions.ReferenceTypeSupport.SimpleReferenceCounting;

public class ActiveRefCountField : CustomDefinedField
{
    public ActiveRefCountField()
        : base(
            null,
            null,
            null,
            SimpleRefCountConstants.CurrentCountFieldName,
            SimpleRefCountConstants.CurrentCountFieldId,
            new IlTypeName(typeof(int).FullName!),
            false,
            [])
    {
    }

    public override CustomCodeStatementSet? GetCustomDeclaration()
    {
        return null;
    }
}