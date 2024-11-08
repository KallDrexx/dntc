namespace Dntc.Common.Syntax.Statements;

/// <summary>
/// Represents a set of statements that's manually defined C code.
/// </summary>
public record CustomCodeStatementSet(string RawCode) : CStatementSet
{
    public override async Task WriteAsync(StreamWriter writer)
    {
        await writer.WriteAsync(RawCode);
    }
}