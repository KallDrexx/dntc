using Dntc.Common.Conversion.EvaluationStack;
using Mono.Cecil.Cil;

namespace Dntc.Common.Conversion.OpCodeHandlers;

internal class LdArgHandlers : IOpCodeFnFactory
{
    public IReadOnlyDictionary<Code, OpCodeHandlerFn> Get()
    {
        return new Dictionary<Code, OpCodeHandlerFn>
        {
            { Code.Ldarg, CreateHandlerFn(null) },
            { Code.Ldarg_0, CreateHandlerFn(0) },
            { Code.Ldarg_1, CreateHandlerFn(1) },
            { Code.Ldarg_2, CreateHandlerFn(2) },
            { Code.Ldarg_3, CreateHandlerFn(3) },
        };
    }

    private static OpCodeHandlerFn CreateHandlerFn(int? hardCodedIndex)
    {
        OpCodeHandlerFn fn;
        if (hardCodedIndex == null)
        {
            fn = context =>
            {
                if (context.Operand is not int index)
                {
                    var message = $"Expected ldarg operand of int, instead was '{context.Operand?.GetType().FullName}'";
                    throw new ArgumentException(message);
                }

                LoadArgument(index, context.EvaluationStack);
                return new ValueTask();
            };
        }
        else
        {
            fn = context =>
            {
                LoadArgument(hardCodedIndex.Value, context.EvaluationStack);
                return new ValueTask();
            };
        }

        return fn;
    }
    
    private static void LoadArgument(int index, Stack<EvaluationStackItem> evaluationStackItems)
    {
        evaluationStackItems.Push(new MethodParameter(index));
    }
}
