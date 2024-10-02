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
                var message = $"Method '{method.Id.Value}' contains op code '{instruction.OpCode.Code}' " +
                              "but no handler exists for it";
                throw new InvalidOperationException(message);
            }

            var targets = GetBranchTargets(instruction);
            if (targets.Any())
            {
                branchTargets.AddRange(targets);
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
            case Code.Newobj:
            case Code.Call:
            case Code.Callvirt:
            {
                switch (instruction.Operand)
                {
                    case MethodDefinition definition:
                        return new IlMethodId(definition.FullName);

                    case MethodReference reference:
                        return new IlMethodId(reference.FullName);

                    default:
                        throw new NotSupportedException(instruction.Operand.GetType().FullName);
                }
            }

            default:
                return null;
        }
    }

    private static IReadOnlyList<int> GetBranchTargets(Instruction instruction)
    {
        switch (instruction.OpCode.Code)
        {
            case Code.Br:
            case Code.Br_S:
            case Code.Brfalse:
            case Code.Brtrue:
            case Code.Brfalse_S:
            case Code.Brtrue_S:
            case Code.Beq:
            case Code.Beq_S:
            case Code.Ble:
            case Code.Ble_S:
            case Code.Ble_Un:
            case Code.Ble_Un_S:
            case Code.Blt:
            case Code.Blt_S:
            case Code.Blt_Un:
            case Code.Blt_Un_S:
            case Code.Bge:
            case Code.Bge_S:
            case Code.Bge_Un:
            case Code.Bge_Un_S:
            case Code.Bgt:
            case Code.Bgt_S:
            case Code.Bgt_Un:
            case Code.Bgt_Un_S:
            case Code.Bne_Un:
            case Code.Bne_Un_S:
            {
                var target = (Instruction)instruction.Operand;
                return [target.Offset];
            }

            case Code.Switch:
            {
                var targets = (Instruction[])instruction.Operand;
                return targets.Select(x => x.Offset).ToArray();
            }

            default:
                return [];
        }
    }
}