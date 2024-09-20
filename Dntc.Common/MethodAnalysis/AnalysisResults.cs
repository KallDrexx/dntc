namespace Dntc.Common.MethodAnalysis;

public class AnalysisResults
{
    public IReadOnlySet<int> BranchTargetOffsets { get; }
    public IReadOnlySet<IlMethodId> CalledMethods { get; }
    
    public AnalysisResults(IReadOnlyList<int> branchTargetOffsets, IReadOnlySet<IlMethodId> calledMethods)
    {
        CalledMethods = calledMethods;
        BranchTargetOffsets = branchTargetOffsets.ToHashSet();
    }
}