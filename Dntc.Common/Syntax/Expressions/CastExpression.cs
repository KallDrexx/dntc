using Dntc.Common.Conversion;

namespace Dntc.Common.Syntax.Expressions;

/// <summary>
/// Casts the result of the expression to the specified type.
/// </summary>
public record CastExpression(CBaseExpression Expression, TypeConversionInfo CastTo) : CBaseExpression(false)
{
    // NOTE: Not sure if we need to determine if the type we are casting to is a pointer or not. This
    // all depends on how reference types end up looking.

    public override TypeConversionInfo ResultingType => CastTo;

    public override async ValueTask WriteCodeStringAsync(StreamWriter writer)
    {
        await writer.WriteAsync($"(({CastTo.NameInC})");
        await Expression.WriteCodeStringAsync(writer);
        await writer.WriteAsync(")");
    }

    public override CBaseExpression? ReplaceExpression(CBaseExpression search, CBaseExpression replacement)
    {
        var inner = ReplaceExpression(Expression, search, replacement);
        return inner != null ? this with { Expression = inner } : null;
    }
}