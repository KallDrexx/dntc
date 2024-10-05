namespace Dntc.Common.Syntax.Expressions;

/// <summary>
/// Applies an operator (such as `+`, `>`, `==`) to two expressions for evaluation.
/// </summary>
// NOTE: I don't think any math operations would end up doing pointer arithmetic, but not completely sure.
public record TwoExpressionEvalExpression(CBaseExpression Left, string Operator, CBaseExpression Right) : CBaseExpression(false);