using Dntc.Common.Conversion.OpCodeHandlers;
using Dntc.Common.Definitions;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Dntc.Common.MethodAnalysis;

public class MethodAnalyzer
{
    private readonly KnownOpcodeHandlers _knownOpcodeHandlers = new();
    
    public AnalysisResults Analyze(DotNetDefinedMethod method)
    {
        var branchTargets = new List<int>();
        var calledMethods = new HashSet<IlMethodId>();
        foreach (var instruction in method.Definition.Body.Instructions)
        {
            if (_knownOpcodeHandlers.Get(instruction.OpCode.Code) == null)
            {
                var message = $"Function contains op code '{instruction.OpCode.Code}' but no handler exists for it";
                throw new InvalidOperationException(message);
            }
            
            var branchTarget = GetBranchTarget(instruction);
            if (branchTarget != null)
            {
                branchTargets.Add(branchTarget.Value);
            }

            var callTarget = GetCallTarget(instruction);
            if (callTarget != null)
            {
                calledMethods.Add(callTarget.Value);
            }
        }

        return new AnalysisResults(branchTargets, calledMethods);
    }

    private static IlMethodId? GetCallTarget(Instruction instruction)
    {
        switch (instruction.OpCode.Code)
        {
            case Code.Call:
            case Code.Callvirt:
            {
                var definition = (MethodDefinition)instruction.Operand;
                return new IlMethodId(definition.FullName);
            }
                
            default:
                return null;
        }
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