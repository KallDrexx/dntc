namespace Dntc.Common.Syntax.Expressions;

public record DereferencedValueExpression(CBaseExpression Expression) : CBaseExpression(false)
{
    public override async ValueTask WriteCodeStringAsync(StreamWriter writer)
    {
        if (Expression.ProducesAPointer)
        {
            await writer.WriteAsync("(*");
            await Expression.WriteCodeStringAsync(writer);
            await writer.WriteAsync(")");
        }
        else
        {
            // Not a pointer so nothing to dereference
            await Expression.WriteCodeStringAsync(writer);
        }
    }
}