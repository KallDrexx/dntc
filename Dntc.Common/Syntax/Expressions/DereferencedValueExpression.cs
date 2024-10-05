namespace Dntc.Common.Syntax.Expressions;

public record DereferencedValueExpression(CBaseExpression Expression) : CBaseExpression(false)
{
    public override async ValueTask WriteCodeString(StreamWriter writer)
    {
        if (Expression.ProducesAPointer)
        {
            await writer.WriteAsync("(*");
            await Expression.WriteCodeString(writer);
            await writer.WriteAsync(")");
        }
        else
        {
            // Not a pointer so nothing to dereference
            await Expression.WriteCodeString(writer);
        }
    }
}