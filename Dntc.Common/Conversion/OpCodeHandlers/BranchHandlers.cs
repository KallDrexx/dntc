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
        { Code.Beq, CreateComparisonFn("==") },
        { Code.Beq_S, CreateComparisonFn("==") },
        { Code.Ble, CreateComparisonFn("<=") },
        { Code.Ble_S, CreateComparisonFn("<=") },
        { Code.Ble_Un, CreateComparisonFn("<=") },
        { Code.Ble_Un_S, CreateComparisonFn("<=") },
        { Code.Blt, CreateComparisonFn("<") },
        { Code.Blt_S, CreateComparisonFn("<") },
        { Code.Blt_Un, CreateComparisonFn("<") },
        { Code.Blt_Un_S, CreateComparisonFn("<") },
        { Code.Bge, CreateComparisonFn(">=") },
        { Code.Bge_S, CreateComparisonFn(">=") },
        { Code.Bge_Un, CreateComparisonFn(">=") },
        { Code.Bge_Un_S, CreateComparisonFn(">=") },
        { Code.Bgt, CreateComparisonFn(">") },
        { Code.Bgt_S, CreateComparisonFn(">") },
        { Code.Bgt_Un, CreateComparisonFn(">") },
        { Code.Bgt_Un_S, CreateComparisonFn(">") },
        { Code.Bne_Un, CreateComparisonFn("!=") },
        { Code.Bne_Un_S, CreateComparisonFn("!=") },
    };

    private static OpCodeHandlerFn CreateSimpleBoolCheckFn(bool isTrueCheck)
    {
        return context => HandleBranchBool(isTrueCheck, context);
    }

    private static OpCodeHandlerFn CreateComparisonFn(string comparison)
    {
        return context => HandleBranchComparison(comparison, context);
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

    private static async ValueTask HandleBranchComparison(string comparison, OpCodeHandlingContext context)
    {
        var instruction = (Instruction)context.Operand;
        var items = context.EvaluationStack.PopCount(2);
        var value2 = items[0];
        var value1 = items[1];

        await context.Writer.WriteLineAsync($"\tif ({value1.Dereferenced} {comparison} {value2.Dereferenced}) {{");
        await context.Writer.WriteLineAsync($"\t\tgoto {CodeGenerator.OffsetLabel(instruction.Offset)};");
        await context.Writer.WriteLineAsync("\t}");
    }
}