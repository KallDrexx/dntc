namespace Dntc.Common.Syntax.Statements;

public record LocalDeclarationStatementSet(Variable Variable) : CStatementSet
{
    public override async Task WriteAsync(StreamWriter writer)
    {
        await writer.WriteLineAsync($"\t{Variable.NativeTypeName()} {Variable.Name} = {{0}};");
    }
}