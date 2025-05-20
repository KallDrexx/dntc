using Dntc.Common.Conversion;
using Dntc.Common.Definitions.ReferenceTypeSupport;
using Dntc.Common.Syntax.Expressions;

namespace Dntc.Common.Syntax.Statements;

public record GcUntrackFunctionCallStatement(
    CBaseExpression VariableToUntrack,
    ConversionCatalog Catalog)
    : CStatementSet
{
    public override async Task WriteAsync(StreamWriter writer)
    {
        var untrackMethod = Catalog.Find(ReferenceTypeConstants.GcUntrackMethodId);
        var baseType = Catalog.Find(ReferenceTypeConstants.ReferenceTypeBaseId);

        await writer.WriteAsync($"\t{untrackMethod.NameInC}(({baseType.NameInC}**)&");
        await VariableToUntrack.WriteCodeStringAsync(writer);
        await writer.WriteLineAsync(");");
    }
}