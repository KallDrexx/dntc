namespace Dntc.Common.Syntax.Expressions;

public record VariableValueExpression(Variable Variable) : CBaseExpression(Variable.IsPointer)
{
    public override async ValueTask WriteCodeStringAsync(StreamWriter writer)
    {
        await writer.WriteAsync($"{Variable.Name}");
    }
}
