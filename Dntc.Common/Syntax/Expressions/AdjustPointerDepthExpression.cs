using Dntc.Common.Conversion;

namespace Dntc.Common.Syntax.Expressions;

public record AdjustPointerDepthExpression(
    CBaseExpression Expression, 
    int TargetPointerDepth) : CBaseExpression(TargetPointerDepth)
{
    public override TypeConversionInfo ResultingType => Expression.ResultingType;

    public override async ValueTask WriteCodeStringAsync(StreamWriter writer)
    {
        if (Expression.PointerDepth == TargetPointerDepth)
        {
            await Expression.WriteCodeStringAsync(writer);
        }
        else
        {
            await writer.WriteAsync("(");

            if (Expression.PointerDepth > TargetPointerDepth)
            {
                for (var x = Expression.PointerDepth; x > TargetPointerDepth; x--)
                {
                    await writer.WriteAsync("*");
                }
            }
            else
            {
                for (var x = Expression.PointerDepth; x < TargetPointerDepth; x++)
                {
                    await writer.WriteAsync("&");
                }
            }

            await Expression.WriteCodeStringAsync(writer);
            await writer.WriteAsync(")");
        }
    }

    public override CBaseExpression? ReplaceExpression(CBaseExpression search, CBaseExpression replacement)
    {
        var inner = ReplaceExpression(Expression, search, replacement);
        return inner != null ? this with { Expression = inner } : null;
    }
}