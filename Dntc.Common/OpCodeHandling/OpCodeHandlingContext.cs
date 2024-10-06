using Dntc.Common.Conversion;
using Mono.Cecil.Cil;

namespace Dntc.Common.OpCodeHandling;

public class OpCodeHandlingContext
{
    public Instruction CurrentInstruction { get; } 
    public ExpressionStack ExpressionStack { get; }
    public MethodConversionInfo CurrentMethod { get; }
    
    public OpCodeHandlingContext(Instruction currentInstruction, ExpressionStack expressionStack, MethodConversionInfo currentMethod)
    {
        CurrentInstruction = currentInstruction;
        ExpressionStack = expressionStack;
        CurrentMethod = currentMethod;
    }
}