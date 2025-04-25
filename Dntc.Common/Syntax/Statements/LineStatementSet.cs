using Dntc.Common.Syntax.Statements.Generators;
using Mono.Cecil.Cil;

namespace Dntc.Common.Syntax.Statements;

public record LineStatementSet(LineInfoMode Mode, int IlOffset, SequencePoint SequencePoint) : CStatementSet
{
    public override async Task WriteAsync(StreamWriter writer)
    {
        switch (Mode)
        {
            case LineInfoMode.Directives:
                await writer.WriteLineAsync($"#line {SequencePoint.StartLine} \"{SequencePoint.Document.Url}\" // [{SequencePoint.StartLine} {SequencePoint.StartColumn} - {SequencePoint.EndLine} {SequencePoint.EndColumn}]");
                writer.NewLine = "";
                break;
            case LineInfoMode.Comments:
                await writer.WriteLineAsync($"// \"{SequencePoint.Document.Url}\" [{SequencePoint.StartLine} {SequencePoint.StartColumn} - {SequencePoint.EndLine} {SequencePoint.EndColumn}]");
                writer.NewLine = "";
                break;
            case LineInfoMode.None:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}