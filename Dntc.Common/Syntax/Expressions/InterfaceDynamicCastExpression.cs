using Dntc.Common.Conversion;

namespace Dntc.Common.Syntax.Expressions;

public record InterfaceDynamicCastExpression(VariableValueExpression Variable, CBaseExpression Local, uint TypeCode) : CBaseExpression(Variable.ResultingType.IsPointer)
{
    // NOTE: Not sure if we need to determine if the type we are casting to is a pointer or not. This
    // all depends on how reference types end up looking.

    public override TypeConversionInfo ResultingType => Variable.ResultingType;

    public override async ValueTask WriteCodeStringAsync(StreamWriter writer)
    {
        await writer.WriteAsync($"({ResultingType.NameInC}*)dynamic_cast_interface((ReferenceType_Base*)");
        await Local.WriteCodeStringAsync(writer);
        await writer.WriteAsync($", {TypeCode})");
    }

    public override CBaseExpression? ReplaceExpression(CBaseExpression search, CBaseExpression replacement)
    {
        throw new NotImplementedException();
        //var inner = ReplaceExpression(Variable, search, replacement);
        //return inner != null ? this with { Expression = inner } : null;
    }
}