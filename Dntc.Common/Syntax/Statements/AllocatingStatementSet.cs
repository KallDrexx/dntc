namespace Dntc.Common.Syntax.Statements;

public record AllocatingStatementSet(Variable Variable) : CStatementSet
{
    public override async Task WriteAsync(StreamWriter writer)
    {
        await writer.WriteLineAsync($"\t{Variable.Name} = ({Variable.NativeTypeName()}) malloc(sizeof({Variable.Type.NativeNameWithPossiblePointer()}));");
        await writer.WriteLineAsync(
            $"\tmemset({Variable.Name}, 0, sizeof({Variable.Type.NativeNameWithPossiblePointer()}));");
    }
}