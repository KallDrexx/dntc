using Dntc.Common.Conversion;

namespace Dntc.Common.Syntax.Expressions;

public record ArrayIndexExpression(
    CBaseExpression Array,
    DereferencedValueExpression Index,
    TypeConversionInfo ValueType)
    : CBaseExpression(true) // Indexing operation in C is essentially pointer arithmetic, so it produces a pointer
{
    public override TypeConversionInfo ResultingType => ValueType;
    
    public override async ValueTask WriteCodeStringAsync(StreamWriter writer)
    {
        await Array.WriteCodeStringAsync(writer);
        await writer.WriteAsync("[");
        await Index.WriteCodeStringAsync(writer);
        await writer.WriteAsync("]");
    }

    public override CBaseExpression? ReplaceExpression(CBaseExpression search, CBaseExpression replacement)
    {
        var inner = ReplaceExpression(Array, search, replacement);
        return inner != null ? this with { Array = inner } : null;
    }
}