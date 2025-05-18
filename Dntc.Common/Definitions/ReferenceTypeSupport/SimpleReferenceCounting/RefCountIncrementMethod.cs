using Dntc.Common.Conversion;
using Dntc.Common.Syntax.Statements;

namespace Dntc.Common.Definitions.ReferenceTypeSupport.SimpleReferenceCounting;

public class RefCountIncrementMethod : CustomDefinedMethod
{
    public RefCountIncrementMethod()
        : base(
            ReferenceTypeConstants.RcIncrementMethodId,
            new IlTypeName(typeof(void).FullName!),
            ReferenceTypeConstants.IlNamespace,
            ReferenceTypeConstants.HeaderFileName,
            ReferenceTypeConstants.SourceFileName,
            new CFunctionName("DntcReferenceTypeBase_Rc_Increment"),
            [
                new Parameter(ReferenceTypeConstants.ReferenceTypeBaseId, "referenceType", true),
            ])
    {
    }

    public override CustomCodeStatementSet? GetCustomDeclaration()
    {
        return new CustomCodeStatementSet(
            $"void {NativeName}({ReferenceTypeConstants.ReferenceTypeBaseName} *referenceType)");
    }

    public override CStatementSet? GetCustomImplementation(ConversionCatalog catalog)
    {
        return new CustomCodeStatementSet($@"
    referenceType->{SimpleRefCountConstants.CurrentCountFieldName}++;
");
    }
}