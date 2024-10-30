namespace Dntc.Common.OpCodeHandling;

public class OpCodeAnalysisResult
{
    public InvokedMethod? CalledMethod { get; }
    public HashSet<IlTypeName> ReferencedTypes { get; } = [];

    public OpCodeAnalysisResult()
    {
        CalledMethod = null;
    }
    
    public OpCodeAnalysisResult(InvokedMethod? calledMethod)
    {
        CalledMethod = calledMethod;
    }

    public OpCodeAnalysisResult(IReadOnlyList<IlTypeName> referencedTypes)
    {
        foreach (var type in referencedTypes)
        {
            ReferencedTypes.Add(type);
        }
    }
}