using Dntc.Common.Syntax.Statements.Generators;
using Mono.Cecil.Cil;

namespace Dntc.Common.Syntax.Statements;

public record LineStatementSet(LineInfoMode Mode, int IlOffset, SequencePoint SequencePoint) : CStatementSet
{
    public override async Task WriteAsync(StreamWriter writer)
    {
        string lineToWrite = Mode switch
        {
            LineInfoMode.Directives => $"#line {SequencePoint.StartLine} \"{SequencePoint.Document.Url}\" // [{SequencePoint.StartLine} {SequencePoint.StartColumn} - {SequencePoint.EndLine} {SequencePoint.EndColumn}]",
            LineInfoMode.Comments => $"// \"{SequencePoint.Document.Url}\" [{SequencePoint.StartLine} {SequencePoint.StartColumn} - {SequencePoint.EndLine} {SequencePoint.EndColumn}]",
            LineInfoMode.None => string.Empty,
            _ => throw new ArgumentOutOfRangeException()
        };

        if (!string.IsNullOrEmpty(lineToWrite))
        {
            await writer.WriteLineAsync(lineToWrite.TrimStart('\n'));
            writer.NewLine = "";
        }
    }
}