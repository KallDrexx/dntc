using Dntc.Common.Conversion;
using Dntc.Common.Definitions;

namespace Dntc.Common.Syntax.Expressions;

public record MethodCallExpression(
    CBaseExpression FnExpression,
    IReadOnlyList<MethodConversionInfo.Parameter> Parameters,
    IReadOnlyList<CBaseExpression> Arguments,
    TypeConversionInfo ReturnType,
    bool IsVirtualCall = false, bool IsInterface = false)
    : CBaseExpression(ReturnType.IsPointer)
{
    // Note: Right now method can only return value types. That may change depending on how
    // reference types end up being handled

    public override TypeConversionInfo ResultingType => ReturnType;

    public override async ValueTask WriteCodeStringAsync(StreamWriter writer)
    {
        if (IsVirtualCall)
        {
            if (IsInterface)
            {
                var thisExpression = Arguments[0];

                var isInterfacePtrCall =
                    thisExpression.ResultingType.OriginalTypeDefinition is DotNetDefinedType dotNetDefinedType &&
                    dotNetDefinedType.Definition.IsInterface;
                
                await thisExpression.WriteCodeStringAsync(writer);
                var targetInterface = Parameters[0].ConversionInfo.NameInC.Value;
                await writer.WriteAsync("->");

                if (!isInterfacePtrCall)
                {
                    await writer.WriteAsync(targetInterface);
                    await writer.WriteAsync(".");
                }

                await FnExpression.WriteCodeStringAsync(writer);
                await writer.WriteAsync($"(");
                await WriteParametersAsync(writer);
                await writer.WriteAsync(")");
            }
            else
            {
                var thisExpression = Arguments[0];
                var targetExpression = Parameters[0];
                await writer.WriteAsync($"(({targetExpression.ConversionInfo.NativeNameWithPointer()})");
                await thisExpression.WriteCodeStringAsync(writer);
                await writer.WriteAsync(")->");
                await FnExpression.WriteCodeStringAsync(writer);
                await writer.WriteAsync($"(({targetExpression.ConversionInfo.NativeNameWithPointer()})");
                await WriteParametersAsync(writer);
                await writer.WriteAsync(")");
            }
        }
        else
        {
            await FnExpression.WriteCodeStringAsync(writer);
            await writer.WriteAsync("(");

            await WriteParametersAsync(writer);

            await writer.WriteAsync(")");
        }
    }

    private async ValueTask WriteParametersAsync(StreamWriter writer)
    {
        for (var x = 0; x < Arguments.Count; x++)
        {
            if (x > 0) await writer.WriteAsync(", ");

            if (Parameters[x].IsReference &&
                Parameters[x].ConversionInfo.NameInC != Arguments[x].ResultingType.NameInC)
            {
                await writer.WriteAsync($"({Parameters[x].ConversionInfo.NameInC}*)");
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