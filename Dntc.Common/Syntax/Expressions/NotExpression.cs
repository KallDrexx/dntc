namespace Dntc.Common.Syntax.Expressions;

/// <summary>
/// Negates the specified expression
/// </summary>
public record NotExpression(CBaseExpression Expression) : CBaseExpression(false);