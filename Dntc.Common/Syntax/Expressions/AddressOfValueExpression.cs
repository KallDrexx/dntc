namespace Dntc.Common.Syntax.Expressions;

public record AddressOfValueExpression(CBaseExpression Inner) : CBaseExpression(true);
