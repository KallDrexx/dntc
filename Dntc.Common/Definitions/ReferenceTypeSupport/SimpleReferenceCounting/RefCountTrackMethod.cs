using Dntc.Common.Conversion;
using Dntc.Common.OpCodeHandling;
using Dntc.Common.Syntax.Statements;

namespace Dntc.Common.Definitions.ReferenceTypeSupport.SimpleReferenceCounting;

public class RefCountTrackMethod : CustomDefinedMethod
{
    public sealed override List<InvokedMethod> InvokedMethods { get; }

    public RefCountTrackMethod(IMemoryManagementActions memoryManagement)
        : base(
            ReferenceTypeConstants.GcTrackMethodId,
            new IlTypeName(typeof(void).FullName!),
            ReferenceTypeConstants.IlNamespace,
            ReferenceTypeConstants.HeaderFileName,
            ReferenceTypeConstants.SourceFileName,
            new CFunctionName("DntcReferenceTypeBase_Gc_Track"),
            [
                new Parameter(ReferenceTypeConstants.ReferenceTypeBaseId, "referenceType", true, false),
            ])
    {
        // While untrack won't be called by track, we should include it as an invoked method so the
        // dependency graph will pick it up. It's much easier to heuristically know that untrack is needed if
        // track is needed.
        InvokedMethods = [new InvokedMethod(ReferenceTypeConstants.GcUntrackMethodId)];
        ReferencedHeaders = memoryManagement.RequiredHeaders;
    }

    public override CustomCodeStatementSet? GetCustomDeclaration(ConversionCatalog catalog)
    {
        return new CustomCodeStatementSet(
            $"void {NativeName}({ReferenceTypeConstants.ReferenceTypeBaseName} *referenceType)");
    }

    public override CStatementSet? GetCustomImplementation(ConversionCatalog catalog)
    {
        return new CustomCodeStatementSet($@"
    if (referenceType == NULL) return;
    referenceType->{SimpleRefCountConstants.CurrentCountFieldName}++;
");
    }
}