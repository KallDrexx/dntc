using Mono.Cecil.Cil;

namespace Dntc.Common.Conversion.OpCodeHandlers;

public interface IOpCodeHandler
{
    Code OpCode { get; }

    ValueTask HandleOpCodeAsync(
        object operand,
        Stack<EvaluationStackItem> evaluationStack,
        StreamWriter writer);
}