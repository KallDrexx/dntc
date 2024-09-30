using Mono.Cecil.Cil;

namespace Dntc.Common.Conversion.OpCodeHandlers;

internal class LoadLocalHandlers : IOpCodeFnFactory
{
    public IReadOnlyDictionary<Code, OpCodeHandlerFn> Get() => new Dictionary<Code, OpCodeHandlerFn>
    {
        { Code.Ldloc, CreateLdLocFn(null) },
        { Code.Ldloc_S, CreateLdLocFn(null) },
        { Code.Ldloc_0, CreateLdLocFn(0) },
        { Code.Ldloc_1, CreateLdLocFn(1) },
        { Code.Ldloc_2, CreateLdLocFn(2) },
        { Code.Ldloc_3, CreateLdLocFn(3) },
        { Code.Ldloca, HandleLdLocAddress },
        { Code.Ldloca_S, HandleLdLocAddress },
    };

    private static OpCodeHandlerFn CreateLdLocFn(int? hardCodedIndex)
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

                return HandleLdLoc(index, context);
            };
        }

        return context => HandleLdLoc(hardCodedIndex.Value, context);
    }

    private static ValueTask HandleLdLoc(int index, OpCodeHandlingContext context)
    {
        if (context.Variables.Locals.Count <= index)
        {
            var message = $"Requested to put local #{index} on the stack, " +
                          $"but only {context.Variables.Locals.Count} locals are defined";

            throw new InvalidOperationException(message);
        }

        var local = context.Variables.Locals[index];
        var itemText = local.IsPointer
            ? $"(*{local.Name})"
            : local.Name;
        
        var item = new EvaluationStackItem(itemText, local.IsPointer);
        context.EvaluationStack.Push(item);

        return new ValueTask();
    }

    private static ValueTask HandleLdLocAddress(OpCodeHandlingContext context)
    {
        var variable = (VariableDefinition)context.Operand;
        var index = variable.Index;
        if (context.Variables.Locals.Count <= index)
        {
            var message = $"Requested to load local #{index} but only ${context.Variables.Locals.Count} are defiend";
            throw new InvalidOperationException(message);
        }
        
        var local = context.Variables.Locals[index];
        var itemText = local.IsPointer
            ? local.Name
            : $"(&{local.Name}";
        
        var item = new EvaluationStackItem(itemText, true);
        context.EvaluationStack.Push(item);

        return new ValueTask();
    }
}