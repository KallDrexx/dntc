using Mono.Cecil.Cil;

namespace Dntc.Common.Conversion.OpCodeHandlers;

internal class LdLocHandler : IOpCodeFnFactory
{
    public IReadOnlyDictionary<Code, OpCodeHandlerFn> Get() => new Dictionary<Code, OpCodeHandlerFn>
    {
        { Code.Ldloc, CreateFn(null) },
        { Code.Ldloc_0, CreateFn(0) },
        { Code.Ldloc_1, CreateFn(1) },
        { Code.Ldloc_2, CreateFn(2) },
        { Code.Ldloc_3, CreateFn(3) },
    };

    private static OpCodeHandlerFn CreateFn(int? hardCodedIndex)
    {
        if (hardCodedIndex == null)
        {
            return context =>
            {
                if (context.Operand is not int index)
                {
                    var message = $"Expected ldloc operand of int, instead was '{context.Operand?.GetType().FullName}'";
                    throw new ArgumentException(message);
                }

                return Handle(index, context);
            };
        }

        return context => Handle(hardCodedIndex.Value, context);
    }

    private static ValueTask Handle(int index, OpCodeHandlingContext context)
    {
        if (context.Variables.Locals.Count <= index)
        {
            var message = $"Requested to put local #{index} on the stack, " +
                          $"but only {context.Variables.Locals.Count} locals are defined";

            throw new InvalidOperationException(message);
        }

        var item = new EvaluationStackItem(context.Variables.Locals[index].Name);
        context.EvaluationStack.Push(item);

        return new ValueTask();
    }
}