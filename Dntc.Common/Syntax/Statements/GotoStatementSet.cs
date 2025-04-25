using Dntc.Common.Conversion;
using Dntc.Common.Definitions;
using Mono.Cecil.Cil;

namespace Dntc.Common.Syntax.Statements;

/// <summary>
/// Statement that jumps to the specified IL instruction offset
/// </summary>
/// <param name="IlOffset"></param>
public record GotoStatementSet(int IlOffset, MethodConversionInfo CurrentMethod) : CStatementSet
{
    public override async Task WriteAsync(StreamWriter writer)
    {
        await writer.WriteAsync($"\tgoto {Utils.IlOffsetToLabel(IlOffset, CurrentMethod)};");
    }
}

public record LineStatementSet : CStatementSet
{
    public LineStatementSet(int IlOffset, SequencePoint SequencePoint)
    {
        this.IlOffset = IlOffset;
        this.SequencePoint = SequencePoint;
    }

    public override async Task WriteAsync(StreamWriter writer)
    {
        await writer.WriteLineAsync();
        await writer.WriteLineAsync($"#line {SequencePoint.StartLine} \"{SequencePoint.Document.Url}\" // {SequencePoint.StartLine}:{SequencePoint.EndLine},{SequencePoint.StartColumn}:{SequencePoint.EndColumn}");
    }

    public int IlOffset { get; init; }
    public SequencePoint SequencePoint { get; init; }

    public void Deconstruct(out int IlOffset, out SequencePoint SequencePoint)
    {
        IlOffset = this.IlOffset;
        SequencePoint = this.SequencePoint;
    }
}