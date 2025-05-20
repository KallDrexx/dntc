using Dntc.Common.Conversion;
using Dntc.Common.Definitions;
using Dntc.Common.Definitions.ReferenceTypeSupport;
using Mono.Cecil.Cil;

namespace Dntc.Common.OpCodeHandling;

public record HandleContext(
    Instruction CurrentInstruction,
    ExpressionStack ExpressionStack,
    MethodConversionInfo CurrentMethodConversion,
    DotNetDefinedMethod CurrentDotNetMethod,
    ConversionCatalog ConversionCatalog,
    DefinitionCatalog DefinitionCatalog,
    IMemoryManagementActions MemoryManagementActions,
    IReadOnlyList<Variable> ReferenceTypeVariables);

public record AnalyzeContext(
    Instruction CurrentInstruction,
    DotNetDefinedMethod CurrentMethod,
    IMemoryManagementActions MemoryManagementActions);

public interface IOpCodeHandler
{
    OpCodeHandlingResult Handle(HandleContext context);

    OpCodeAnalysisResult Analyze(AnalyzeContext context);
}