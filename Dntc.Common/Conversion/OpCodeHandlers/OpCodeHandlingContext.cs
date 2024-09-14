namespace Dntc.Common.Conversion.OpCodeHandlers;

public class OpCodeHandlingContext
{
    public object Operand { get; set; }
    public Stack<EvaluationStackItem> EvaluationStack { get; } = new();
    public IReadOnlyList<string> ArgumentNames { get; }
    public LocalNameCollection LocalNameCollection { get; }
    public StreamWriter Writer { get; }

    public OpCodeHandlingContext(
        IReadOnlyList<string> argumentNames, 
        LocalNameCollection localNameCollection, 
        StreamWriter writer)
    {
        ArgumentNames = argumentNames;
        LocalNameCollection = localNameCollection;
        Writer = writer;
    }
}