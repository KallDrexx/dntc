using Dntc.Common.Syntax.Expressions;

namespace Dntc.Common.OpCodeHandling;

public class ExpressionStack
{
    // Manually implement a stack via a list, as we will need to do periodic index based searching
    // and replacements.
    private readonly List<CBaseExpression> _expressions = [];

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
            var index = _expressions.Count - 1;
            var expression = _expressions[index];
            _expressions.RemoveAt(index);
            results.Add(expression);
        }

        return results;
    }

    public void Push(CBaseExpression expression)
    {
        _expressions.Add(expression);
    }

    /// <summary>
    /// Searches for any expression in the stack and replaces it with the specified expression.
    /// </summary>
    /// <returns>True if an expression was replaced, false if the searched expression was not in the stack</returns>
    public bool ReplaceExpression(CBaseExpression search, CBaseExpression replace)
    {
        var referencesFound = false;
        for (var x = 0; x < _expressions.Count; x++)
        {
            var expression = _expressions[x];
            if (expression == search)
            {
                _expressions[x] = replace;
                referencesFound = true;
                continue;
            }

            var mutatedExpression = expression.ReplaceExpression(search, replace);
            if (mutatedExpression != null)
            {
                _expressions[x] = mutatedExpression;
            }
        }

        return referencesFound;
    }
}