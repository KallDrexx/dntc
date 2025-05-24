using Dntc.Common.Conversion;
using Dntc.Common.Syntax.Expressions;

namespace Dntc.Common.Syntax.Statements;

public record GcUntrackIfNotNullStatementSet(CBaseExpression Expression, ConversionCatalog Catalog) : CStatementSet
{
    public override async Task WriteAsync(StreamWriter writer)
    {
        if (!Expression.ResultingType.IsReferenceType)
        {
            var message = $"Only reference types can be untracked, but the passed in " +
                          $"expression produces {Expression.ResultingType.IlName}";

            throw new InvalidOperationException(message);
        }

        var gcUntrack = new GcUntrackFunctionCallStatement(Expression, Catalog);

        await writer.WriteAsync("\tif (");
        await Expression.WriteCodeStringAsync(writer);
        await writer.WriteLineAsync(" != NULL) {");
        await writer.WriteAsync("\t");
        await gcUntrack.WriteAsync(writer);
        await writer.WriteAsync("\n\t}\n");
    }
}