using Dntc.Common.Conversion.EvaluationStack;
using Mono.Cecil.Cil;

namespace Dntc.Common.Conversion.OpCodeHandlers;

internal class StLocOpHandlers : IOpCodeHandlerFnFactory
{
    public IReadOnlyDictionary<Code, OpCodeHandlerFn> Get() => new Dictionary<Code, OpCodeHandlerFn>
    {
        { Code.Stloc, CreateFn(null) },
        { Code.Stloc_0, CreateFn(0) },
        { Code.Stloc_1, CreateFn(0) },
        { Code.Stloc_2, CreateFn(0) },
        { Code.Stloc_3, CreateFn(0) },
    };

    private static OpCodeHandlerFn CreateFn(int? hardCodedIndex)
    {
        if (hardCodedIndex == null)
        {
            return context =>
            {
                if (context.Operand is not int index)
                {
                    var message = $"Expected stloc operand of int, instead was '{context.Operand?.GetType().FullName}'";
                    throw new ArgumentException(message);
                }

                return HandleStore(index, context);
            };
        }

        return context => HandleStore(hardCodedIndex.Value, context);
    }
    
    private static async ValueTask HandleStore(int localIndex, OpCodeHandlingContext context)
    {
        var local = context.Variables.Locals[localIndex];
        await context.Writer.WriteAsync($"{local.Name} = ");

        var stackItem = context.EvaluationStack.Pop();
        switch (stackItem)
        {
            case LocalVariable localVariable:
                var localAssignSource = context.Variables.Locals[localVariable.Index];
                await context.Writer.WriteLineAsync($"{localAssignSource.Name};");
                break;
            
            case MethodParameter parameter:
                var paramAssignSource = context.Variables.Parameters[parameter.Index];
                await context.Writer.WriteLineAsync($"{paramAssignSource.Name};");
                break;
            
            case AddResultItem addResult:
                await context.Writer.WriteLineAsync($"{addResult.first.Name} + {addResult.second.Name};");
                break;
            
            default:
                throw new NotSupportedException(stackItem.GetType().FullName);
        }
    }
}