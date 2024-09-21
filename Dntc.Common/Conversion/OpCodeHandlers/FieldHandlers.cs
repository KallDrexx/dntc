using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Dntc.Common.Conversion.OpCodeHandlers;

internal class FieldHandlers : IOpCodeFnFactory
{
    public IReadOnlyDictionary<Code, OpCodeHandlerFn> Get() => new Dictionary<Code, OpCodeHandlerFn>
    {
        { Code.Stfld, HandleStfld },
        { Code.Ldfld, HandleLdFld },
    };

    private static async ValueTask HandleStfld(OpCodeHandlingContext context)
    {
        var field = (FieldDefinition)context.Operand;
        var items = context.EvaluationStack.PopCount(2);
        var value = items[0];
        var obj = items[1];

        await context.Writer.WriteLineAsync($"\t{obj.Text}.{field.Name} = {value.Text};");
    }

    private static ValueTask HandleLdFld(OpCodeHandlingContext context)
    {
        var field = (FieldDefinition)context.Operand;
        var items = context.EvaluationStack.PopCount(1);
        var obj = items[0];

        var newItemString = $"({obj.Text}.{field.Name})";
        context.EvaluationStack.Push(new EvaluationStackItem(newItemString));

        return new ValueTask();
    }
}