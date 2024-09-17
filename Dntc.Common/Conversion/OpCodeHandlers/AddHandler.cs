using Dntc.Common.Conversion.EvaluationStack;
using Mono.Cecil.Cil;

namespace Dntc.Common.Conversion.OpCodeHandlers;

internal class AddHandler : IOpCodeFnFactory
{
    public IReadOnlyDictionary<Code, OpCodeHandlerFn> Get() => new Dictionary<Code, OpCodeHandlerFn>
    {
        { Code.Add, HandleAdd }
    };

    private static ValueTask HandleAdd(OpCodeHandlingContext context)
    {
        if (context.EvaluationStack.Count < 2)
        {
            const string message = "Add requires two evaluation stack items, only one is present";
            throw new InvalidOperationException(message);
        }

        var item2 = GetNextStackItemInfo(context);
        var item1 = GetNextStackItemInfo(context);
        
        context.EvaluationStack.Push(new AddResultItem(item1, item2));

        return new ValueTask();
    }

    private static Variable GetNextStackItemInfo(OpCodeHandlingContext context)
    {
        var item = context.EvaluationStack.Pop();
        switch (item)
        {
            case LocalVariable local:
                if (context.Variables.Locals.Count < local.Index)
                {
                    var message = $"Local variable with index {local.Index} was on the stack, " +
                                  $"but only {context.Variables.Locals.Count} exist";
                    throw new InvalidOperationException(message);
                }
                
                return context.Variables.Locals[local.Index];
            
            case MethodParameter param:
                if (context.Variables.Parameters.Count < param.Index)
                {
                    var message = $"Method parameters with index {param.Index} was on the stack, " +
                                  $"but only {context.Variables.Parameters.Count} exist";
                    throw new InvalidOperationException(message);
                }
                
                return context.Variables.Parameters[param.Index];
            
            default:
                throw new NotSupportedException(item.GetType().FullName);
        }
    }
}