using Dntc.Common.Conversion;

namespace Dntc.Common.Syntax.Expressions;

public record VariableValueExpression(Variable Variable) : CBaseExpression(Variable.PointerDepth)
{
    public override TypeConversionInfo ResultingType => Variable.Type;

    public override async ValueTask WriteCodeStringAsync(StreamWriter writer)
    {
        await writer.WriteAsync($"{Variable.Name}");
    }

    public override CBaseExpression? ReplaceExpression(CBaseExpression search, CBaseExpression replacement)
    {
        // This does not contain an expression
        return null;
    }
}
