using Dntc.Common.Conversion;
using Dntc.Common.Syntax.Statements;

namespace Dntc.Common.Definitions.ReferenceTypeSupport.SimpleReferenceCounting;

public class RefCountTrackMethod : CustomDefinedMethod
{
    public RefCountTrackMethod()
        : base(
            ReferenceTypeConstants.GcTrackMethodId,
            new IlTypeName(typeof(void).FullName!),
            ReferenceTypeConstants.IlNamespace,
            ReferenceTypeConstants.HeaderFileName,
            ReferenceTypeConstants.SourceFileName,
            new CFunctionName("DntcReferenceTypeBase_Gc_Track"),
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