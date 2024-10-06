namespace Dntc.Common.Syntax.Expressions;

/// <summary>
/// Negates the specified expression
/// </summary>
public record NotExpression(CBaseExpression Expression) : CBaseExpression(false)
{
    public override async ValueTask WriteCodeStringAsync(StreamWriter writer)
    {
        await writer.WriteAsync("!");
        await Expression.WriteCodeStringAsync(writer);
    }
}