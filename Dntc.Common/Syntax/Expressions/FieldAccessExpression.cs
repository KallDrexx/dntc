using Dntc.Common.Conversion;

namespace Dntc.Common.Syntax.Expressions;

public record FieldAccessExpression(
    CBaseExpression OwningObject,
    Variable Field) : CBaseExpression(Field.IsPointer)
{
    public override TypeConversionInfo ResultingType => Field.Type;

    public override async ValueTask WriteCodeStringAsync(StreamWriter writer)
    {
        await writer.WriteAsync("(");
        await OwningObject.WriteCodeStringAsync(writer);

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

    public override CBaseExpression? ReplaceExpression(CBaseExpression search, CBaseExpression replacement)
    {
        var owningObject = ReplaceExpression(OwningObject, search, replacement);
        return owningObject != null ? this with {OwningObject = owningObject} : null;
    }
}