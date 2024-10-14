using Dntc.Common.Conversion;

namespace Dntc.Common.Syntax.Expressions;

public record ZeroValuedObjectExpression(TypeConversionInfo TypeInfo) : CBaseExpression(false)
{
    public override TypeConversionInfo? ResultingType => TypeInfo;

    public override async ValueTask WriteCodeStringAsync(StreamWriter writer)
    {
        await writer.WriteAsync($"(({TypeInfo.NameInC}){{0}})");
    }

    public override CBaseExpression? ReplaceExpression(CBaseExpression search, CBaseExpression replacement)
    {
        // This does not contain an expression
        return null;
    }
}