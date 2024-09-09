namespace Dntc.Common.Conversion.OpCodeHandling;

public abstract record EvaluationStackItem();

public record MethodArgument(int Index) : EvaluationStackItem;
public record LocalVariable(int Index) : EvaluationStackItem;
