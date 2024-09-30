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
        var items = context.EvaluationStack.PopCount(2);
        var item2 = items[0];
        var item1 = items[1];

        var comparisonString = $"({item1.TextDerefed} {operatorString} {item2.TextDerefed})";
        context.EvaluationStack.Push(new EvaluationStackItem(comparisonString, false));

        return new ValueTask();
    }
}