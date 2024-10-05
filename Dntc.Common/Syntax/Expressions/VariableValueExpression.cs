namespace Dntc.Common.Syntax.Expressions;

public record VariableValueExpression(Variable Variable) : CBaseExpression(Variable.IsPointer);
