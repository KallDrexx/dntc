using Dntc.Common.Conversion;

namespace Dntc.Common.Syntax.Expressions;

public record MethodCallExpression(
    CBaseExpression FnExpression,
    IReadOnlyList<CBaseExpression> Arguments,
    IReadOnlyList<int> ArgumentCastDepths,
    TypeConversionInfo ReturnType)
    : CBaseExpression(ReturnType.IsPointer)
{
    // Note: Right now method can only return value types. That may change depending on how
    // reference types end up being handled

    public override TypeConversionInfo ResultingType => ReturnType;

    public override async ValueTask WriteCodeStringAsync(StreamWriter writer)
    {
        await FnExpression.WriteCodeStringAsync(writer);
        await writer.WriteAsync("(");

        for (var x = 0; x < Arguments.Count; x++)
        {
            if (x > 0) await writer.WriteAsync(", ");

            var param = Arguments[x];

            if (ArgumentCastDepths.Count == Arguments.Count && ArgumentCastDepths[x] > 0 && Arguments[x] is VariableValueExpression varExpr)
            {
                varExpr.CastDepth = ArgumentCastDepths[x];
            }
            
            await param.WriteCodeStringAsync(writer);

            if (Arguments[x] is VariableValueExpression expression)
            {
                expression.CastDepth = 0;
            }
        }

        await writer.WriteAsync(")");
    }

    public override CBaseExpression? ReplaceExpression(CBaseExpression search, CBaseExpression replacement)
    {
        var newFn = ReplaceExpression(FnExpression, search, replacement);
        return newFn != null ? this with { FnExpression = newFn } : null;
    }
}