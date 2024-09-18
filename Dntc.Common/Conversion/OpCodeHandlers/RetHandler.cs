using Dntc.Common.Conversion.EvaluationStack;
using Mono.Cecil.Cil;

namespace Dntc.Common.Conversion.OpCodeHandlers;

internal class RetHandler : IOpCodeFnFactory
{
    public IReadOnlyDictionary<Code, OpCodeHandlerFn> Get() => new Dictionary<Code, OpCodeHandlerFn>
    {
        { Code.Ret, Handle },
    };

    private static async ValueTask Handle(OpCodeHandlingContext context)
    {
        await context.Writer.WriteAsync("\t return");
        
        // I'm assuming it's ok to have more than one item on the stack in some situations
        // when a return is encountered.
        if (context.EvaluationStack.TryPop(out var item))
        {
            switch (item)
            {
                case LocalVariable localVariable:
                    var local = context.Variables.Locals[localVariable.Index];
                    await context.Writer.WriteLineAsync($" {local.Name};");
                    break;
                
                case MethodParameter parameter:
                    var param = context.Variables.Parameters[parameter.Index];
                    await context.Writer.WriteLineAsync($" {param.Name};");
                    break;
                
                case AddResultItem addResultItem:
                    var first = addResultItem.first;
                    var second = addResultItem.second;
                    await context.Writer.WriteLineAsync($" {first.Name} + {second.Name};");
                    break;
                
                default:
                    throw new NotSupportedException(item.GetType().FullName);
            }
        }
        else
        {
            await context.Writer.WriteLineAsync(";");
        }
    }
}