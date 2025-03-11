using Dntc.Common.Conversion;
using Dntc.Common.Syntax.Expressions;

namespace Dntc.Common.Syntax.Statements;

/// <summary>
/// If the expression evaluates to true, then jump to the specified IL offset. Otherwise, fall through
/// to the next statement set.
/// </summary>
public record IfConditionJumpStatementSet(
    CBaseExpression Expression,
    int IlOffset,
    MethodConversionInfo CurrentMethod) : CStatementSet
{
    public override async Task WriteAsync(StreamWriter writer)
    {
        await writer.WriteAsync("\tif (");
        await Expression.WriteCodeStringAsync(writer);
        await writer.WriteLineAsync(") {");
        await writer.WriteLineAsync($"\t\tgoto {Utils.IlOffsetToLabel(IlOffset, CurrentMethod)};");
        await writer.WriteLineAsync("\t}");
    }
}