using Mono.Cecil.Cil;

namespace Dntc.Common.Conversion.OpCodeHandlers;

internal class SwitchHandler : IOpCodeFnFactory
{
    public IReadOnlyDictionary<Code, OpCodeHandlerFn> Get() => new Dictionary<Code, OpCodeHandlerFn>
    {
        { Code.Switch, HandleSwitch },
    };

    private static async ValueTask HandleSwitch(OpCodeHandlingContext context)
    {
        var items = context.EvaluationStack.PopCount(1);
        var targets = (Instruction[])context.Operand;

        await context.Writer.WriteLineAsync($"\tswitch ({items[0].Dereferenced}) {{");
        for (var x = 0; x < targets.Length; x++)
        {
            await context.Writer.WriteLineAsync($"\t\tcase {x}:");
            await context.Writer.WriteLineAsync($"\t\t\tgoto {CodeGenerator.OffsetLabel(targets[x].Offset)};");
            await context.Writer.WriteLineAsync();
        }
        await context.Writer.WriteLineAsync("\t}");
    }
}