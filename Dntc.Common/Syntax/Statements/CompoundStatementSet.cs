namespace Dntc.Common.Syntax.Statements;

/// <summary>
/// Represents multiple statement sets in order
/// </summary>
public record CompoundStatementSet(IReadOnlyList<CStatementSet> Statements) : CStatementSet
{
    public override async Task WriteAsync(StreamWriter writer)
    {
        foreach (var statement in Statements)
        {
            await statement.WriteAsync(writer);
        }
    }

    /// <summary>
    /// Returns each individual statement contained within the compound statement individually. This happens
    /// recursively.
    ///
    /// Since statements are not guaranteed to get starting and ending IL offsets (they are actually set via the
    /// planned file converter), it's not a guarantee that we have valid offsets for statements within the compound,
    /// and thus we give each statement the same starting and ending IL offsets as the compound we are flattening.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<CStatementSet> Flatten()
    {
        foreach (var statement in Statements)
        {
            if (statement is CompoundStatementSet innerCompound)
            {
                foreach (var innerStatement in innerCompound.Flatten())
                {
                    yield return innerStatement with { StartingIlOffset = StartingIlOffset, LastIlOffset = LastIlOffset};
                }
            }
            else
            {
                yield return statement with { StartingIlOffset = StartingIlOffset, LastIlOffset = LastIlOffset};
            }
        }
    }
}