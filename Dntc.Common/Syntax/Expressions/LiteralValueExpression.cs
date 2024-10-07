namespace Dntc.Common.Syntax.Expressions;

public record LiteralValueExpression(string Value) : CBaseExpression(false)
{
    public override async ValueTask WriteCodeStringAsync(StreamWriter writer)
    {
        await writer.WriteAsync(Value);
    }
}
