using Dntc.Common.Syntax.Statements.Generators;

namespace Dntc.Common.Syntax.Statements;

public record EndLineStatementSet(DebugInfoMode Mode, int IlOffset) : CStatementSet
{
    public override async Task WriteAsync(StreamWriter writer)
    {
        if (Mode != DebugInfoMode.None)
        {
            writer.NewLine = Environment.NewLine;
            await writer.WriteLineAsync();
            await writer.WriteLineAsync();
        }
    }
}