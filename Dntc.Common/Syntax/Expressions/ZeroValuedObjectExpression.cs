using Dntc.Common.Conversion;

namespace Dntc.Common.Syntax.Expressions;

public record ZeroValuedObjectExpression(TypeConversionInfo TypeInfo) : CBaseExpression(false);