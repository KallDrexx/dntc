namespace Dntc.Common.Syntax.Statements;

/// <summary>
/// Represents multiple statement sets in order
/// </summary>
public record CompoundStatementSet(IReadOnlyList<CStatementSet> Statements) : CStatementSet
{
    public override async Task WriteAsync(StreamWriter writer)
    {
        foreach (var statement in Statements)
        {
            await statement.WriteAsync(writer);
        }
    }
}