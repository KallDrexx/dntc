using Dntc.Common.Definitions;

namespace Dntc.Common.Conversion.OpCodeHandlers;

internal class OpCodeHandlingContext
{
    public object Operand { get; set; } = default!;
    public Stack<EvaluationStackItem> EvaluationStack { get; } = new();
    public IReadOnlyList<string> ArgumentNames { get; }
    public VariableCollection Variables { get; }
    public StreamWriter Writer { get; }
    public ConversionCatalog ConversionCatalog { get; }
    public DefinitionCatalog DefinitionCatalog { get; }

    public OpCodeHandlingContext(
        IReadOnlyList<string> argumentNames, 
        VariableCollection variables, 
        StreamWriter writer, 
        ConversionCatalog conversionCatalog, 
        DefinitionCatalog definitionCatalog)
    {
        ArgumentNames = argumentNames;
        Variables = variables;
        Writer = writer;
        ConversionCatalog = conversionCatalog;
        DefinitionCatalog = definitionCatalog;
    }
}