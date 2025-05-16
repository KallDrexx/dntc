using Dntc.Common.Syntax.Statements;

namespace Dntc.Common.Definitions.ReferenceTypeSupport.ReferenceCounting;

public class ReferenceCounterField : CustomDefinedField
{
    public ReferenceCounterField()
        : base(
            null,
            null,
            null,
            ReferenceCountConstants.CounterFieldName,
            ReferenceCountConstants.CounterIlFieldId,
            ReferenceCountConstants.CounterIlTypeName,
            false,
            [])
    {
    }

    public override CustomCodeStatementSet GetCustomDeclaration()
    {
        return new CustomCodeStatementSet($"{ReferenceCountConstants.CounterTypeNativeName} {NativeName}");
    }
}