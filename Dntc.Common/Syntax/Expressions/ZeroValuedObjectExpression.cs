using Dntc.Common.Conversion;

namespace Dntc.Common.Syntax.Expressions;

public record ZeroValuedObjectExpression(TypeConversionInfo TypeInfo) : CBaseExpression(false)
{
    public override async ValueTask WriteCodeStringAsync(StreamWriter writer)
    {
        await writer.WriteAsync($"(({TypeInfo.NameInC}){{0}})");
    }
}