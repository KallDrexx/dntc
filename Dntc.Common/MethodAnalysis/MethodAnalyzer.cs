using Dntc.Common.Definitions;
using Mono.Cecil.Cil;

namespace Dntc.Common.MethodAnalysis;

public class MethodAnalyzer
{
    public AnalysisResults Analyze(DotNetDefinedMethod method)
    {
        var branchTargets = new List<int>();
        foreach (var instruction in method.Definition.Body.Instructions)
        {
            var branchTarget = GetBranchTarget(instruction);
            if (branchTarget != null)
            {
                branchTargets.Add(branchTarget.Value);
            }
        }

        return new AnalysisResults(branchTargets);
    }

    private static int? GetBranchTarget(Instruction instruction)
    {
        switch (instruction.OpCode.Code)
        {
            case Code.Br:
            case Code.Br_S:
            case Code.Brfalse:
            case Code.Brtrue:
            case Code.Brfalse_S:
            case Code.Brtrue_S:
            {
                var target = (Instruction)instruction.Operand;
                return target.Offset;
            }
            
            default:
                return null;
        }
    }
}