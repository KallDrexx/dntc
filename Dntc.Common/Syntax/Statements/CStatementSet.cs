namespace Dntc.Common.Syntax.Statements;

/// <summary>
/// Represents one or more holistic representations of code that can be executed to have an action
/// performed. Statements do not provide a value and thus cannot be nested together.
/// </summary>
public abstract record CStatementSet
{
    /// <summary>
    /// The first IL Offset that this statement encompasses
    /// </summary>
    public required int StartingIlOffset { get; init; }
    
    /// <summary>
    /// The last IL offset (inclusively) that this statement encompasses
    /// </summary>
    public required int LastIlOffset { get; init; }

    public abstract Task WriteAsync(StreamWriter writer);
}
