using Dntc.Common.Conversion;
using Dntc.Common.Definitions;
using Mono.Cecil.Cil;

namespace Dntc.Common.OpCodeHandling;

public record HandleContext(
    Instruction CurrentInstruction,
    ExpressionStack ExpressionStack,
    MethodConversionInfo CurrentMethodConversion,
    DotNetDefinedMethod CurrentDotNetMethod,
    ConversionCatalog ConversionCatalog,
    DefinitionCatalog DefinitionCatalog);

public interface IOpCodeHandler
{
    OpCodeHandlingResult Handle(HandleContext context);
}