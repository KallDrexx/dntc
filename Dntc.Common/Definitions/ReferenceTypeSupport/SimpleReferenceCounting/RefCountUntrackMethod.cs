using Dntc.Common.Conversion;
using Dntc.Common.Syntax.Statements;

namespace Dntc.Common.Definitions.ReferenceTypeSupport.SimpleReferenceCounting;

public class RefCountUntrackMethod : CustomDefinedMethod
{
    private readonly IMemoryManagementActions _memoryManagement;

    public RefCountUntrackMethod(IMemoryManagementActions memoryManagement)
        : base(
            ReferenceTypeConstants.GcUntrackMethodId,
            new IlTypeName(typeof(void).FullName!),
            ReferenceTypeConstants.IlNamespace,
            ReferenceTypeConstants.HeaderFileName,
            ReferenceTypeConstants.SourceFileName,
            new CFunctionName("DntcReferenceTypeBase_Gc_Untrack"),
            [
                new Parameter(ReferenceTypeConstants.ReferenceTypeBaseId, "referenceType", true),
            ])
    {
        _memoryManagement = memoryManagement;
    }

    public override CustomCodeStatementSet? GetCustomDeclaration()
    {
        return new CustomCodeStatementSet(
            $"void {NativeName}({ReferenceTypeConstants.ReferenceTypeBaseName} **referenceType)");
    }

    public override CStatementSet? GetCustomImplementation(ConversionCatalog catalog)
    {
        var intType = catalog.Find(new IlTypeName(typeof(int).FullName!));
        var statements = new List<CStatementSet>();
        statements.Add(new CustomCodeStatementSet($@"
    {{ReferenceTypeConstants.ReferenceTypeBaseName}} singlePointerVariable = *referenceType;
    {intType.NameInC} count = --(singlePointerVariable->{SimpleRefCountConstants.CurrentCountFieldName});
    if (count <= 0) {{
"));
        var referenceTypeInfo = catalog.Find(ReferenceTypeConstants.ReferenceTypeBaseId);
        var variable = new Variable(referenceTypeInfo, "singlePointerVariable", true);
        statements.Add(_memoryManagement.FreeCall(variable, catalog));

        statements.Add(new CustomCodeStatementSet($@"
        referenceType = NULL;
}}
"));

        return new CompoundStatementSet(statements);
    }
}