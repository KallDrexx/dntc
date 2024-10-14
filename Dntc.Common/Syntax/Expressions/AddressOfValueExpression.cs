using Dntc.Common.Conversion;

namespace Dntc.Common.Syntax.Expressions;

public record AddressOfValueExpression(CBaseExpression Inner) : CBaseExpression(true)
{
    public override TypeConversionInfo ResultingType => Inner.ResultingType;

    public override async ValueTask WriteCodeStringAsync(StreamWriter writer)
    {
        if (Inner.ProducesAPointer)
        {
            // It's already a pointer, so no change
            await Inner.WriteCodeStringAsync(writer);
        }
        else
        {
            await writer.WriteAsync("(&");
            await Inner.WriteCodeStringAsync(writer);
            await writer.WriteAsync(")");
        }
    }

    public override CBaseExpression? ReplaceExpression(CBaseExpression search, CBaseExpression replacement)
    {
        var inner = ReplaceExpression(Inner, search, replacement);
        return inner != null ? this with { Inner = inner } : null;
    }
}
