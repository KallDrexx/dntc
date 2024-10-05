namespace Dntc.Common.Syntax.Expressions;

public record LiteralValueBaseExpression(string Value) : CBaseExpression(false)
{
    public override async ValueTask WriteCodeString(StreamWriter writer)
    {
        await writer.WriteAsync(Value);
    }
}
