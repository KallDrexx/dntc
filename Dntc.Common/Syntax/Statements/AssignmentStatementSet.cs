using Dntc.Common.Syntax.Expressions;

namespace Dntc.Common.Syntax.Statements;

public record AssignmentStatementSet(CBaseExpression Left, CBaseExpression Right) : CStatementSet
{
    public override async Task WriteAsync(StreamWriter writer)
    {
        await writer.WriteAsync("\t");
        await Left.WriteCodeStringAsync(writer);
        await writer.WriteAsync(" = ");
        await Right.WriteCodeStringAsync(writer);
        await writer.WriteLineAsync(";");
    }
}
