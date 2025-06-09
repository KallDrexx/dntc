using Dntc.Common.Conversion;

namespace Dntc.Common.Syntax.Expressions;

/// <summary>
/// Represents a nestable bit of code that can produce a value.
/// </summary>
public abstract record CBaseExpression
{
    public int PointerDepth { get; }
    public abstract TypeConversionInfo ResultingType { get; }
    
    protected CBaseExpression(int pointerDepth)
    {
        PointerDepth = pointerDepth;
        if (PointerDepth < 0)
        {
            PointerDepth = 0;
        }
    }

    public abstract ValueTask WriteCodeStringAsync(StreamWriter writer);

    /// <summary>
    /// Returns a brand-new expression where the expression being searched for is replaced by the
    /// replacement expression. If the current expression does not contain the expression being searched
    /// for, then no expression is returned.
    /// </summary>
    public abstract CBaseExpression? ReplaceExpression(CBaseExpression search, CBaseExpression replacement);
    
    protected static CBaseExpression? ReplaceExpression(CBaseExpression test, CBaseExpression search, CBaseExpression replacement)
    {
        return test == search ? replacement : test.ReplaceExpression(search, replacement);
    }
}