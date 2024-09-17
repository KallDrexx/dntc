namespace Dntc.Common.MethodAnalysis;

public class AnalysisResults
{
    public IReadOnlySet<int> BranchTargetOffsets { get; }
    
    public AnalysisResults(IReadOnlyList<int> branchTargetOffsets)
    {
        BranchTargetOffsets = branchTargetOffsets.ToHashSet();
    }
}