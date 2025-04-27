using Dntc.Common.Conversion;

namespace Dntc.Common.Syntax.Expressions;

public record VariableValueExpression(Variable Variable) : CBaseExpression(Variable.IsPointer)
{
    public override TypeConversionInfo ResultingType => Variable.Type;
    
    public int CastDepth { get; set; }

    public override async ValueTask WriteCodeStringAsync(StreamWriter writer)
    {
        if (CastDepth > 0)
        {
            await writer.WriteAsync($"&");
        }
        
        await writer.WriteAsync($"{Variable.Name}");

        if (CastDepth > 0)
        {
            await writer.WriteAsync($"->base");
        }

        for (int i = 1; i < CastDepth; i++)
        {
            await writer.WriteAsync($".base");
        }
    }

    public override CBaseExpression? ReplaceExpression(CBaseExpression search, CBaseExpression replacement)
    {
        // This does not contain an expression
        return null;
    }
}
