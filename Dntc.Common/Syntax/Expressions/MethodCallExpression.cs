using Dntc.Common.Conversion;

namespace Dntc.Common.Syntax.Expressions;

public record MethodCallExpression(
    CBaseExpression FnExpression,
    IReadOnlyList<MethodConversionInfo.Parameter> Parameters,
    IReadOnlyList<CBaseExpression> Arguments,
    TypeConversionInfo ReturnType,
    ConversionCatalog Catalog,
    bool IsVirtualCall = false)
    : CBaseExpression(ReturnType.IsPointer)
{
    public override TypeConversionInfo ResultingType => ReturnType;

    public override async ValueTask WriteCodeStringAsync(StreamWriter writer)
    {
        if (IsVirtualCall)
        {
            var thisExpression = Arguments[0];
            var targetExpression = Parameters[0];
            var targetExpressionInfo = Catalog.Find(targetExpression.TypeName);
            await writer.WriteAsync($"(({targetExpressionInfo.NativeNameWithPointer()})");
            await thisExpression.WriteCodeStringAsync(writer);
            await writer.WriteAsync(")->");
            await FnExpression.WriteCodeStringAsync(writer);

            await writer.WriteAsync("(");
            await WriteParametersAsync(writer);
            await writer.WriteAsync(")");
        }
        else
        {
            await FnExpression.WriteCodeStringAsync(writer);
            
            await writer.WriteAsync("(");
            await WriteParametersAsync(writer);
            await writer.WriteAsync(")");
        }
    }

    private async ValueTask WriteParametersAsync(StreamWriter writer, int startIndex = 0)
    {
        for (var x = startIndex; x < Arguments.Count; x++)
        {
            if (x > 0) await writer.WriteAsync(", ");

            var paramInfo = Catalog.Find(Parameters[x].TypeName);
            if (Parameters[x].IsReference && paramInfo.NameInC != Arguments[x].ResultingType.NameInC)
            {
                await writer.WriteAsync($"({paramInfo.NameInC}*)");
            }

            var param = Arguments[x];

            await param.WriteCodeStringAsync(writer);
        }
    }

    public override CBaseExpression? ReplaceExpression(CBaseExpression search, CBaseExpression replacement)
    {
        var newFn = ReplaceExpression(FnExpression, search, replacement);
        return newFn != null ? this with { FnExpression = newFn } : null;
    }
}