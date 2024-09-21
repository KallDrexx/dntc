using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Dntc.Common.Conversion.OpCodeHandlers;

internal class StFldHandlers : IOpCodeFnFactory
{
    public IReadOnlyDictionary<Code, OpCodeHandlerFn> Get() => new Dictionary<Code, OpCodeHandlerFn>
    {
        { Code.Stfld, Handle },
    };

    private static async ValueTask Handle(OpCodeHandlingContext context)
    {
        var field = (FieldDefinition)context.Operand;
        var items = context.EvaluationStack.PopCount(2);
        var value = items[0];
        var obj = items[1];

        await context.Writer.WriteLineAsync($"\t{obj.Text}.{field.Name} = {value.Text};");
    }
}