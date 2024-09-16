namespace Dntc.Common.Conversion.EvaluationStack;

internal record AddResultItem(Variable first, Variable second) : EvaluationStackItem;