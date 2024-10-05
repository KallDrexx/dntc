using Dntc.Common.Conversion;

namespace Dntc.Common.Syntax.Expressions;

public record MethodCallExpression(MethodConversionInfo MethodInfo, IReadOnlyList<CBaseExpression> Parameters)
    // Right now method can only return value types. That may change depending on how reference types end up being handled
    : CBaseExpression(false); 


    