namespace Dntc.Common.Conversion;

internal static class EvaluationStackExtensions
{
    /// <summary>
    /// Returns an array of items from the evaluation stack in the order they were popped
    /// </summary>
    public static IReadOnlyList<EvaluationStackItem> PopCount(this Stack<EvaluationStackItem> stack, int count)
    {
        if (stack.Count < count)
        {
            var message = $"Expected {count} items on the evaluation stack, but only {stack.Count} exists";
            throw new InvalidOperationException(message);
        }

        var result = new EvaluationStackItem[count];
        for (int x = 0; x < count; x++)
        {
            result[x] = stack.Pop();
        }

        return result;
    }
}