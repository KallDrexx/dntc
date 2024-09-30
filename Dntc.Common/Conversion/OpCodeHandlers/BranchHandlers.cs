using Mono.Cecil.Cil;

namespace Dntc.Common.Conversion.OpCodeHandlers;

internal class BranchHandlers : IOpCodeFnFactory
{
    public IReadOnlyDictionary<Code, OpCodeHandlerFn> Get() => new Dictionary<Code, OpCodeHandlerFn>
    {
        { Code.Br, HandleBranch },
        { Code.Br_S, HandleBranch },
        { Code.Brfalse, CreateSimpleBoolCheckFn(false) },
        { Code.Brfalse_S, CreateSimpleBoolCheckFn(false) },
        { Code.Brtrue, CreateSimpleBoolCheckFn(true) },
        { Code.Brtrue_S, CreateSimpleBoolCheckFn(true) },
    };

    private static OpCodeHandlerFn CreateSimpleBoolCheckFn(bool isTrueCheck)
    {
        return context => HandleBranchBool(isTrueCheck, context);
    }

    private static async ValueTask HandleBranch(OpCodeHandlingContext context)
    {
        var instruction = (Instruction)context.Operand;
        await context.Writer.WriteLineAsync($"\tgoto {CodeGenerator.OffsetLabel(instruction.Offset)};");
    }

    private static async ValueTask HandleBranchBool(bool isTrueCheck, OpCodeHandlingContext context)
    {
        var items = context.EvaluationStack.PopCount(1);
        var item = items[0];
        var instruction = (Instruction)context.Operand;
        var check = isTrueCheck ? "" : "!";

        await context.Writer.WriteLineAsync($"\tif ({check}{item.Dereferenced}) {{");
        await context.Writer.WriteLineAsync($"\t\tgoto {CodeGenerator.OffsetLabel(instruction.Offset)};");
        await context.Writer.WriteLineAsync("\t}");
    }
}