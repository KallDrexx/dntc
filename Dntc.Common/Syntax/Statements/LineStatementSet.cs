using Dntc.Common.Syntax.Statements.Generators;
using Mono.Cecil.Cil;

namespace Dntc.Common.Syntax.Statements;

public record LineStatementSet(DebugInfoMode Mode, int IlOffset, SequencePoint SequencePoint) : CStatementSet
{
    public override async Task WriteAsync(StreamWriter writer)
    {
        string lineToWrite = Mode switch
        {
            DebugInfoMode.CLineSourceMaps => $"#line {SequencePoint.StartLine} \"{SequencePoint.Document.Url}\" // [{SequencePoint.StartLine} {SequencePoint.StartColumn} - {SequencePoint.EndLine} {SequencePoint.EndColumn}]",
            DebugInfoMode.LineNumberComments => $"// \"{SequencePoint.Document.Url}\" [{SequencePoint.StartLine} {SequencePoint.StartColumn} - {SequencePoint.EndLine} {SequencePoint.EndColumn}]",
            DebugInfoMode.None => string.Empty,
            _ => throw new ArgumentOutOfRangeException()
        };

        if (!string.IsNullOrEmpty(lineToWrite))
        {
            await writer.WriteLineAsync(lineToWrite.TrimStart('\n'));
            writer.NewLine = "";
        }
    }
}