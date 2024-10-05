namespace Dntc.Common.Syntax.Expressions;

/// <summary>
/// Negates the specified expression
/// </summary>
public record NotExpression(CBaseExpression Expression) : CBaseExpression(false)
{
    public override async ValueTask WriteCodeString(StreamWriter writer)
    {
        await writer.WriteAsync("!");
        await Expression.WriteCodeString(writer);
    }
}