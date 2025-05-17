using Dntc.Common.Definitions.ReferenceTypeSupport.TypeInfo;
using Dntc.Common.Syntax.Statements;

namespace Dntc.Common.Definitions.ReferenceTypeSupport.SimpleReferenceCounting;

public class TypeInfoField() : CustomDefinedField(null,
    null,
    null,
    TypeInfoConstants.TypeInfoFieldName,
    TypeInfoConstants.TypeInfoFieldId,
    TypeInfoConstants.TypeInfoTypeId.AsPointerType(),
    false,
    [])
{
    public override CustomCodeStatementSet? GetCustomDeclaration()
    {
        return null;
    }
}