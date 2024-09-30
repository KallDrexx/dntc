using Dntc.Common.Definitions;

namespace Dntc.Common.Conversion.OpCodeHandlers;

internal class OpCodeHandlingContext
{
    public object Operand { get; set; } = default!;
    public Stack<EvaluationStackItem> EvaluationStack { get; } = new();
    public VariableCollection Variables { get; }
    public StreamWriter Writer { get; }
    public ConversionCatalog ConversionCatalog { get; }
    public DefinitionCatalog DefinitionCatalog { get; }
    public int CurrentInstructionOffset { get; set; }

    public OpCodeHandlingContext(
        VariableCollection variables, 
        StreamWriter writer, 
        ConversionCatalog conversionCatalog, 
        DefinitionCatalog definitionCatalog)
    {
        Variables = variables;
        Writer = writer;
        ConversionCatalog = conversionCatalog;
        DefinitionCatalog = definitionCatalog;
    }
}