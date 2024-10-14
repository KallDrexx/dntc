using Dntc.Common.Conversion;

namespace Dntc.Common.Syntax.Expressions;

/// <summary>
/// Negates the specified expression
/// </summary>
public record NotExpression(CBaseExpression Expression) : CBaseExpression(false)
{
    // Probably can be hardcoded to bool
    public override TypeConversionInfo ResultingType => Expression.ResultingType; 

    public override async ValueTask WriteCodeStringAsync(StreamWriter writer)
    {
        await writer.WriteAsync("!");
        await Expression.WriteCodeStringAsync(writer);
    }

    public override CBaseExpression? ReplaceExpression(CBaseExpression search, CBaseExpression replacement)
    {
        var inner = ReplaceExpression(Expression, search, replacement);
        return inner != null ? this with {Expression = inner} : null;
    }
}