using Dntc.Common.Conversion;

namespace Dntc.Common.Syntax.Expressions;

/// <summary>
/// Casts the result of the expression to the specified type.
/// </summary>
// TODO: Determine if the type we are casting to is a pointer. Not sure if we will need this though.
public record CastExpression(CBaseExpression Expression, TypeConversionInfo CastTo) : CBaseExpression(false);