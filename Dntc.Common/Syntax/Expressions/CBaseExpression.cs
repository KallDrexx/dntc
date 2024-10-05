namespace Dntc.Common.Syntax.Expressions;

/// <summary>
/// Represents a nestable bit of code that can produce a value.
/// </summary>
public abstract record CBaseExpression
{
    public bool ProducesAPointer { get; }
    
    protected CBaseExpression(bool producesAPointer)
    {
        ProducesAPointer = producesAPointer;
    }

    public abstract ValueTask WriteCodeString(StreamWriter writer);
}