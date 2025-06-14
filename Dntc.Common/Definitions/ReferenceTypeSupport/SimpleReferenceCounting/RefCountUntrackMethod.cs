using Dntc.Common.Conversion;
using Dntc.Common.Syntax.Expressions;
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
                new Parameter(ReferenceTypeConstants.ReferenceTypeBaseId, "referenceType", true, false),
            ])
    {
        _memoryManagement = memoryManagement;
        ReferencedHeaders = memoryManagement.RequiredHeaders;
    }

    public override CustomCodeStatementSet? GetCustomDeclaration(ConversionCatalog catalog)
    {
        return new CustomCodeStatementSet(
            $"void {NativeName}({ReferenceTypeConstants.ReferenceTypeBaseName} **referenceType)");
    }

    public override CStatementSet? GetCustomImplementation(ConversionCatalog catalog)
    {
        var intType = catalog.Find(new IlTypeName(typeof(int).FullName!));
        var statements = new List<CStatementSet>();
        statements.Add(new CustomCodeStatementSet("\tif (*referenceType == NULL) return;\n"));

        statements.Add(new CustomCodeStatementSet($@"
    {ReferenceTypeConstants.ReferenceTypeBaseName} *singlePointerVariable = *referenceType;
    {intType.NameInC} count = --(singlePointerVariable->{SimpleRefCountConstants.CurrentCountFieldName});
    if (count <= 0) {{
"));
        var referenceTypeInfo = catalog.Find(ReferenceTypeConstants.ReferenceTypeBaseId);
        var variable = new Variable(referenceTypeInfo, "singlePointerVariable", 1);

        // TODO: Find a way to generalize this so it doesn't have to be manually included
        // in every RC implementation.
        statements.Add(
            new CustomCodeStatementSet(
                "\t\tsinglePointerVariable->PrepForFree(singlePointerVariable);\n"));

        statements.Add(new CustomCodeStatementSet("\t"));
        statements.Add(_memoryManagement.FreeCall(new VariableValueExpression(variable), catalog));

        statements.Add(new CustomCodeStatementSet("\t\t*referenceType = NULL;\n"));
        statements.Add(new CustomCodeStatementSet("\t}\n"));

        return new CompoundStatementSet(statements);
    }
}