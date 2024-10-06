using Dntc.Common.Conversion;
using Mono.Cecil.Cil;

namespace Dntc.Common.OpCodeHandling;

public interface IOpCodeHandler
{
    OpCodeHandlingResult Handle(
        Instruction currentInstruction, 
        ExpressionStack expressionStack, 
        MethodConversionInfo currentMethod,
        ConversionCatalog conversionCatalog);
}