using Dntc.Common.Conversion;

namespace Dntc.Common.Syntax.Expressions;

public record LiteralValueExpression(
    string Value,
    TypeConversionInfo TypeInfo,
    int PointerDepth) : CBaseExpression(PointerDepth)
{
    public override TypeConversionInfo ResultingType => TypeInfo;

    public override async ValueTask WriteCodeStringAsync(StreamWriter writer)
    {
        await writer.WriteAsync(Value);
    }

    public override CBaseExpression? ReplaceExpression(CBaseExpression search, CBaseExpression replacement)
    {
        // No inner expression to replace
        return null;
    }
}
