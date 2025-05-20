using Dntc.Common.Syntax.Statements;

namespace Dntc.Common.Syntax;

public static class SyntaxExtensions
{
    public static IEnumerable<CStatementSet> Flatten(this IEnumerable<CStatementSet> statements)
    {
        foreach (var statement in statements)
        {
            if (statement is CompoundStatementSet compound)
            {
                foreach (var innerStatement in compound.Flatten())
                {
                    yield return innerStatement;
                }
            }

            yield return statement;
        }
    }
}