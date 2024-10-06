namespace Dntc.Common.Syntax.Expressions;

public record AddressOfValueExpression(CBaseExpression Inner) : CBaseExpression(true)
{
    public override async ValueTask WriteCodeStringAsync(StreamWriter writer)
    {
        if (Inner.ProducesAPointer)
        {
            // It's already a pointer, so no change
            await Inner.WriteCodeStringAsync(writer);
        }
        else
        {
            await writer.WriteAsync("(&");
            await Inner.WriteCodeStringAsync(writer);
            await writer.WriteAsync(")");
        }
    }
}
