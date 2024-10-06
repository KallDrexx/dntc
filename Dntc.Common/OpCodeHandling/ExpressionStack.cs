using Dntc.Common.Syntax.Expressions;

namespace Dntc.Common.OpCodeHandling;

public class ExpressionStack
{
    private readonly Stack<CBaseExpression> _expressions = new();

    public int Count => _expressions.Count;

    /// <summary>
    /// Pops the specified number of expressions from the stack. The expressions are returned
    /// in the order they were popped.
    /// </summary>
    public IReadOnlyList<CBaseExpression> Pop(int count)
    {
        if (_expressions.Count < count)
        {
            var message = $"Expected at least {count} items on the stack, but there were {_expressions.Count}";
            throw new ArgumentException(message);
        }

        var results = new List<CBaseExpression>();
        for (var x = 0; x < count; x++)
        {
            results.Add(_expressions.Pop());
        }

        return results;
    }

    public void Push(CBaseExpression expression)
    {
        _expressions.Push(expression);
    }
}