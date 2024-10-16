namespace Dntc.Common.MethodAnalysis;

public class AnalysisResults
{
    public IReadOnlyList<InvokedMethod> CalledMethods { get; }
    
    public AnalysisResults(IReadOnlyList<InvokedMethod> calledMethods)
    {
        CalledMethods = calledMethods;
    }
}