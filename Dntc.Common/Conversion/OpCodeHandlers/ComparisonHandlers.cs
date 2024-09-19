using Mono.Cecil.Cil;

namespace Dntc.Common.Conversion.OpCodeHandlers;

internal class ComparisonHandlers : IOpCodeFnFactory
{
    public IReadOnlyDictionary<Code, OpCodeHandlerFn> Get() => new Dictionary<Code, OpCodeHandlerFn>
    {
        { Code.Clt, CreateFn("<") },
        { Code.Clt_Un, CreateFn("<") },
        { Code.Cgt, CreateFn(">") },
        { Code.Cgt_Un, CreateFn(">") },
        { Code.Ceq, CreateFn("==") },
    };

    private static OpCodeHandlerFn CreateFn(string operatorString)
    {
        return context => Handle(operatorString, context);
    }

    private static ValueTask Handle(string operatorString, OpCodeHandlingContext context)
    {
        if (context.EvaluationStack.Count < 2)
        {
            var message = $"Comparison operation required 2 items on the stack, " +
                          $"but only {context.EvaluationStack.Count} existed";

            throw new InvalidOperationException(message);
        }

        var item2 = context.EvaluationStack.Pop();
        var item1 = context.EvaluationStack.Pop();

        var comparisonString = $"({item1.Text} {operatorString} {item2.Text})";
        context.EvaluationStack.Push(new EvaluationStackItem(comparisonString));

        return new ValueTask();
    }
}