namespace Dntc.Common.Syntax.Statements;

public record AllocatingStatementSet(Variable Variable) : CStatementSet
{
    public override async Task WriteAsync(StreamWriter writer)
    {
        await writer.WriteLineAsync($"\t{Variable.Name} = {Variable.Type.NameInC}__Create();");
    }
}