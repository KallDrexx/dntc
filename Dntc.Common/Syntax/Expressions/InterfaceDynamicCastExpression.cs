using Dntc.Common.Conversion;
using Dntc.Common.Definitions.ReferenceTypeSupport;

namespace Dntc.Common.Syntax.Expressions;

public record InterfaceDynamicCastExpression(VariableValueExpression Variable, CBaseExpression Local, uint TypeCode) : CBaseExpression(Variable.ResultingType.IsPointer)
{
    public override TypeConversionInfo ResultingType => Variable.ResultingType;

    public override async ValueTask WriteCodeStringAsync(StreamWriter writer)
    {
        var referenceTypeBase = ReferenceTypeConstants.ReferenceTypeBaseTypeName;
        await writer.WriteAsync($"({ResultingType.NameInC}*)dynamic_cast_interface(({referenceTypeBase}*)");
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