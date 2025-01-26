using Dntc.Common.Syntax.Expressions;

namespace Dntc.Common.Syntax.Statements;

public record ArrayStoreStatementSet(
    ArrayLengthCheckStatementSet LengthCheckStatementSet,
    ArrayIndexExpression ArrayIndex,
    DereferencedValueExpression Value) : CStatementSet
{
    public override async Task WriteAsync(StreamWriter writer)
    {
        await LengthCheckStatementSet.WriteAsync(writer);
        
        // Assignment
        await writer.WriteAsync("\t");
        await ArrayIndex.WriteCodeStringAsync(writer);
        await writer.WriteAsync(" = ");
        await Value.WriteCodeStringAsync(writer);
        await writer.WriteLineAsync(";");
    }
}