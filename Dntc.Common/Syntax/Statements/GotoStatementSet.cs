using Dntc.Common.Conversion;
using Dntc.Common.Definitions;

namespace Dntc.Common.Syntax.Statements;

/// <summary>
/// Statement that jumps to the specified IL instruction offset
/// </summary>
/// <param name="IlOffset"></param>
public record GotoStatementSet(int IlOffset, MethodConversionInfo CurrentMethod) : CStatementSet
{
    public override async Task WriteAsync(StreamWriter writer)
    {
        await writer.WriteLineAsync($"\tgoto {Utils.IlOffsetToLabel(IlOffset, CurrentMethod)};");
    }
}