namespace Dntc.Common.Syntax.Expressions;

public record AddressOfValueExpression(CBaseExpression Inner) : CBaseExpression(true)
{
    public override async ValueTask WriteCodeString(StreamWriter writer)
    {
        if (Inner.ProducesAPointer)
        {
            // It's already a pointer, so no change
            await Inner.WriteCodeString(writer);
        }
        else
        {
            await writer.WriteAsync("(&");
            await Inner.WriteCodeString(writer);
            await writer.WriteAsync(")");
        }
    }
}
