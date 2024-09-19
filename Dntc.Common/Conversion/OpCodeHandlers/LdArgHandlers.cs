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
        if (hardCodedIndex == null)
        {
            return context =>
            {
                if (context.Operand is not int index)
                {
                    var message = $"Expected ldarg operand of int, instead was '{context.Operand?.GetType().FullName}'";
                    throw new ArgumentException(message);
                }

                LoadArgument(index, context);
                return new ValueTask();
            };
        }

        return context =>
        {
            LoadArgument(hardCodedIndex.Value, context);
            return new ValueTask();
        };
    }
    
    private static void LoadArgument(int index, OpCodeHandlingContext context)
    {
        if (context.ArgumentNames.Count <= index)
        {
            var message = $"Argument index #{index} referenced, but only {context.ArgumentNames.Count} exist";
            throw new InvalidOperationException(message);
        }

        var argument = context.ArgumentNames[index];
        context.EvaluationStack.Push(new EvaluationStackItem(argument));
    }
}
