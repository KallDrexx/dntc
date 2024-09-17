using Mono.Cecil.Cil;

namespace Dntc.Common.Conversion.OpCodeHandlers;

internal class BranchHandlers : IOpCodeFnFactory
{
    public IReadOnlyDictionary<Code, OpCodeHandlerFn> Get() => new Dictionary<Code, OpCodeHandlerFn>
    {
        { Code.Br, HandleBranch },
        { Code.Br_S, HandleBranch },
    };

    private static async ValueTask HandleBranch(OpCodeHandlingContext context)
    {
        var instruction = (Instruction)context.Operand;
        await context.Writer.WriteLineAsync($"\tgoto {CodeGenerator.OffsetLabel(instruction.Offset)};");
    }
}