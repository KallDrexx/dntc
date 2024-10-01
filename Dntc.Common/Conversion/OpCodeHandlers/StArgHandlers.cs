using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Dntc.Common.Conversion.OpCodeHandlers;

internal class StArgHandlers : IOpCodeFnFactory
{
    public IReadOnlyDictionary<Code, OpCodeHandlerFn> Get() => new Dictionary<Code, OpCodeHandlerFn>
    {
        { Code.Starg, HandleStArg },
        { Code.Starg_S, HandleStArg },
    };

    private static async ValueTask HandleStArg(OpCodeHandlingContext context)
    {
        var items = context.EvaluationStack.PopCount(1);
        var value = items[0];

        var argIndex = context.Operand switch
        {
            int intOperand => intOperand,
            ParameterDefinition parameterDefinition => parameterDefinition.Index,
            _ => throw new NotSupportedException(context.Operand.GetType().FullName),
        };

        if (context.Variables.Parameters.Count < argIndex)
        {
            var message = $"starg on argument index {argIndex} is beyond the argument " +
                          $"count of {context.Variables.Parameters.Count}";
            throw new InvalidOperationException(message);
        }

        var argument = context.Variables.Parameters[argIndex];

        await context.Writer.WriteLineAsync($"\t{argument.Name} = {value.Dereferenced};");
    }
}