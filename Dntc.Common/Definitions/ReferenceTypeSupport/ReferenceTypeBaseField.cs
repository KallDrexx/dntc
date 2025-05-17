using Dntc.Common.Syntax.Statements;

namespace Dntc.Common.Definitions.ReferenceTypeSupport;

public class ReferenceTypeBaseField : CustomDefinedField
{
    public ReferenceTypeBaseField()
        : base(
            null,
            null,
            null,
            ReferenceTypeConstants.ReferenceTypeBaseFieldName,
            ReferenceTypeConstants.ReferenceTypeBaseFieldId,
            ReferenceTypeConstants.ReferenceTypeBaseId,
            false,
            [])
    {
    }

    public override CustomCodeStatementSet? GetCustomDeclaration()
    {
        return null;
    }
}