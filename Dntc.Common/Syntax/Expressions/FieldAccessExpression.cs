namespace Dntc.Common.Syntax.Expressions;

public record FieldAccessExpression(CBaseExpression OwningObject, Variable Field) : CBaseExpression(Field.IsPointer)
{
    public override async ValueTask WriteCodeString(StreamWriter writer)
    {
        await writer.WriteAsync("(");
        await OwningObject.WriteCodeString(writer);
        
        if (OwningObject.ProducesAPointer)
        {
            await writer.WriteAsync("->");
        }
        else
        {
            await writer.WriteAsync(".");
        }

        await writer.WriteAsync($"{Field.Name})");
    }
}
