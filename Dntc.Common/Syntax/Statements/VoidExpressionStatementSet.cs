using Dntc.Common.Syntax.Expressions;

namespace Dntc.Common.Syntax.Statements;

/// <summary>
/// A statement that executes an expression without doing anything with the expression's value
/// (e.g. void returning expression, or discarding the results for side effects).
/// </summary>
/// <param name="Expression"></param>
public record VoidExpressionStatementSet(CBaseExpression Expression) : CStatementSet
{
    public override async Task WriteAsync(StreamWriter writer)
    {
        await writer.WriteAsync("\t");
        await Expression.WriteCodeStringAsync(writer);
        await writer.WriteLineAsync(";");
    }
}