using Dntc.Common.Definitions;
using Dntc.Common.OpCodeHandling;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Dntc.Common.MethodAnalysis;

public class MethodAnalyzer
{
    public AnalysisResults Analyze(DotNetDefinedMethod method)
    {
        var calledMethods = new Dictionary<IlMethodId, InvokedMethod>();
        foreach (var instruction in method.Definition.Body.Instructions)
        {
            if (!KnownOpCodeHandlers.OpCodeHandlers.TryGetValue(instruction.OpCode.Code, out _))
            {
                var message = $"Method '{method.Id.Value}' contains op code '{instruction.OpCode.Code}' " +
                              "but no handler exists for it";
                throw new InvalidOperationException(message);
            }

            var callTarget = GetCallTarget(instruction);
            if (callTarget != null)
            {
                calledMethods.TryAdd(callTarget.MethodId, callTarget);
            }
        }

        return new AnalysisResults(calledMethods.Values.ToArray());
    }

    private static InvokedMethod? GetCallTarget(Instruction instruction)
    {
        switch (instruction.OpCode.Code)
        {
            case Code.Newobj:
            case Code.Call:
            case Code.Callvirt:
            {
                if (instruction.Operand is GenericInstanceMethod generic)
                {
                    return new GenericInvokedMethod(
                        new IlMethodId(generic.FullName),
                        new IlMethodId(generic.ElementMethod.FullName),
                        generic.GenericArguments.Select(x => new IlTypeName(x.FullName)).ToArray());
                }

                if (instruction.Operand is MethodReference reference)
                {
                    return new InvokedMethod(new IlMethodId(reference.FullName));
                }

                break;
            }
        }

        return null;
    }
}