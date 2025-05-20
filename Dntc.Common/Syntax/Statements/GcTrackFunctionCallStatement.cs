using Dntc.Common.Conversion;
using Dntc.Common.Definitions.ReferenceTypeSupport;
using Dntc.Common.Syntax.Expressions;

namespace Dntc.Common.Syntax.Statements;

/// <summary>
/// Statement that calls the required function to track specified object by the current
/// garbage collection implementation.
/// </summary>
/// <param name="VariableToIncrement">Expression containing the object to track</param>
/// <param name="Catalog"></param>
public record GcTrackFunctionCallStatement(
    CBaseExpression VariableToIncrement,
    ConversionCatalog Catalog)
    : CStatementSet
{
    public override async Task WriteAsync(StreamWriter writer)
    {
        var baseType = Catalog.Find(ReferenceTypeConstants.ReferenceTypeBaseId);
        var incrementInfo = Catalog.Find(ReferenceTypeConstants.GcTrackMethodId);

        await writer.WriteAsync($"\t{incrementInfo.NameInC}(({baseType.NameInC}*)");
        await VariableToIncrement.WriteCodeStringAsync(writer);
        await writer.WriteLineAsync(");");
    }
}