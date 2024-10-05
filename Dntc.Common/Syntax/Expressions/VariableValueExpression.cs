namespace Dntc.Common.Syntax.Expressions;

public record VariableValueExpression(Variable Variable) : CBaseExpression(Variable.IsPointer)
{
    public override async ValueTask WriteCodeString(StreamWriter writer)
    {
        await writer.WriteAsync($"{Variable.Name}");
    }
}
