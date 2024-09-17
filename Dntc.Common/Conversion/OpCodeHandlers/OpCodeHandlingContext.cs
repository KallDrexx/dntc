using Dntc.Common.Conversion.EvaluationStack;

namespace Dntc.Common.Conversion.OpCodeHandlers;

internal class OpCodeHandlingContext
{
    public object Operand { get; set; } = default!;
    public Stack<EvaluationStackItem> EvaluationStack { get; } = new();
    public IReadOnlyList<string> ArgumentNames { get; }
    public VariableCollection Variables { get; }
    public StreamWriter Writer { get; }

    public OpCodeHandlingContext(
        IReadOnlyList<string> argumentNames, 
        VariableCollection variables, 
        StreamWriter writer)
    {
        ArgumentNames = argumentNames;
        Variables = variables;
        Writer = writer;
    }
}