namespace Dntc.Common.OpCodeHandling;

public class OpCodeAnalysisResult
{
    public InvokedMethod? CalledMethod { get; }

    public OpCodeAnalysisResult()
    {
        CalledMethod = null;
    }
    
    public OpCodeAnalysisResult(InvokedMethod? calledMethod)
    {
        CalledMethod = calledMethod;
    }
}