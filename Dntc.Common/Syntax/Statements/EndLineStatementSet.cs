using Dntc.Common.Syntax.Statements.Generators;

namespace Dntc.Common.Syntax.Statements;

public record EndLineStatementSet(LineInfoMode Mode, int IlOffset) : CStatementSet
{
    public override async Task WriteAsync(StreamWriter writer)
    {
        if (Mode != LineInfoMode.None)
        {
            writer.NewLine = Environment.NewLine;
            await writer.WriteLineAsync();
            await writer.WriteLineAsync();
        }
    }
}