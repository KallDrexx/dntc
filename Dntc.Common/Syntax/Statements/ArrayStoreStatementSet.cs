using Dntc.Common.Syntax.Expressions;

namespace Dntc.Common.Syntax.Statements;

public record ArrayStoreStatementSet(
    ArrayIndexExpression ArrayIndex,
    CBaseExpression Value) : CStatementSet
{
    public override async Task WriteAsync(StreamWriter writer)
    {
        // Assignment
        await writer.WriteAsync("\t");
        await ArrayIndex.WriteCodeStringAsync(writer);
        await writer.WriteAsync(" = ");
        await Value.WriteCodeStringAsync(writer);
        await writer.WriteLineAsync(";");
    }
}