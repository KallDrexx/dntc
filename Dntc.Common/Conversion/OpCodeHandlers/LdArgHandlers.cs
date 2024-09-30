using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Dntc.Common.Conversion.OpCodeHandlers;

internal class LdArgHandlers : IOpCodeFnFactory
{
    public IReadOnlyDictionary<Code, OpCodeHandlerFn> Get()
    {
        return new Dictionary<Code, OpCodeHandlerFn>
        {
            { Code.Ldarg, CreateHandlerFn(null, false) },
            { Code.Ldarg_S, CreateHandlerFn(null, false) },
            { Code.Ldarg_0, CreateHandlerFn(0, false) },
            { Code.Ldarg_1, CreateHandlerFn(1, false) },
            { Code.Ldarg_2, CreateHandlerFn(2, false) },
            { Code.Ldarg_3, CreateHandlerFn(3, false) },
           
            // I think just loading the parameter name on the eval stack is ok for transpiling
            { Code.Ldarga, CreateHandlerFn(null, true) },
            { Code.Ldarga_S, CreateHandlerFn(null, true) },
        };
    }

    private static OpCodeHandlerFn CreateHandlerFn(int? hardCodedIndex, bool loadAddress)
    {
        if (hardCodedIndex == null)
        {
            return context =>
            {
                var index = context.Operand switch
                {
                    int intIndex => intIndex,
                    ParameterDefinition paramDef => paramDef.Index,
                    _ => throw new ArgumentException(
                        $"Unknown ldarg operand type of {context.Operand.GetType().FullName}"),
                };

                return LoadArgument(index, loadAddress, context);
            };
        }

        return context => LoadArgument(hardCodedIndex.Value, loadAddress, context);
    }
    
    private static ValueTask LoadArgument(int index, bool loadAddress, OpCodeHandlingContext context)
    {
        if (context.Variables.Parameters.Count <= index)
        {
            var message = $"Argument index #{index} referenced, but only {context.Variables.Parameters.Count} exist";
            throw new InvalidOperationException(message);
        }

        var parameter = context.Variables.Parameters[index];
        
        // My assumption based on what I'm seeing is that if the MSIL wants an address, but an argument 
        // is not passed by reference, then it will use ldarga (loadAddress == true). If the argument
        // is passed by reference, then it will use ldarg (loadAddress == false) and use the indirect
        // load bytecodes to load the value after. 
        // 
        // I don't know if it's ever valid to see ldarga when a passed by reference value is specified?
        var createdItem = (loadAddress, parameter.IsPointer) switch
        {
            (true, false) => new EvaluationStackItem($"(&{parameter.Name})", true),
            (false, true) => new EvaluationStackItem($"({parameter.Name})", true),
            (false, false) => new EvaluationStackItem(parameter.Name, false),
            
            (true, true) => throw new NotSupportedException("ldarg called for passed by ref parameter"),
        };
        
        context.EvaluationStack.Push(createdItem);
        return new ValueTask();
    }
}
