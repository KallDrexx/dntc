using Mono.Cecil.Cil;

namespace Dntc.Common.Conversion.OpCodeHandlers;

internal class BranchHandlers : IOpCodeFnFactory
{
    public IReadOnlyDictionary<Code, OpCodeHandlerFn> Get() => new Dictionary<Code, OpCodeHandlerFn>
    {
        { Code.Br, HandleBranch },
        { Code.Br_S, HandleBranch },
        { Code.Brfalse, HandleBranchFalse },
        { Code.Brfalse_S, HandleBranchFalse },
    };

    private static async ValueTask HandleBranch(OpCodeHandlingContext context)
    {
        var instruction = (Instruction)context.Operand;
        await context.Writer.WriteLineAsync($"\tgoto {CodeGenerator.OffsetLabel(instruction.Offset)};");
    }

    private static async ValueTask HandleBranchFalse(OpCodeHandlingContext context)
    {
        if (context.EvaluationStack.Count == 0)
        {
            var message = "brfalse requires a value on the evaluation stack";
            throw new NotImplementedException(message);
        }
        
        var instruction = (Instruction)context.Operand;
        var item = context.EvaluationStack.Pop();

        await context.Writer.WriteLineAsync($"\tif (!{item.Text}) {{");
        await context.Writer.WriteLineAsync($"\t\tgoto {CodeGenerator.OffsetLabel(instruction.Offset)};");
        await context.Writer.WriteLineAsync("\t}");
    }
}