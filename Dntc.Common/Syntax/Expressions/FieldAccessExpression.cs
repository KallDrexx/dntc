namespace Dntc.Common.Syntax.Expressions;

public record FieldAccessExpression(CBaseExpression OwningObject, Variable Field) : CBaseExpression(Field.IsPointer);
