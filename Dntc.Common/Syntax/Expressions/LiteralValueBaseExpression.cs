namespace Dntc.Common.Syntax.Expressions;

public record LiteralValueBaseExpression(string Value) : CBaseExpression(false)
{
    public override async ValueTask WriteCodeStringAsync(StreamWriter writer)
    {
        await writer.WriteAsync(Value);
    }
}
