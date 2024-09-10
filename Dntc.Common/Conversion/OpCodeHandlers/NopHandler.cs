using Mono.Cecil.Cil;

namespace Dntc.Common.Conversion.OpCodeHandlers;

public class NopHandler : IOpCodeHandler
{
    public Code OpCode => Code.Nop;
    
    public ValueTask HandleOpCodeAsync(object operand, Stack<EvaluationStackItem> evaluationStack, StreamWriter writer)
    {
        // Do nothing
        return new ValueTask();
    }
}