using Dntc.Common.Syntax.Expressions;

namespace Dntc.Common.Syntax.Statements;

public record ArrayLengthCheckStatementSet(
    CBaseExpression ArrayLengthField,
    CBaseExpression Array,
    CBaseExpression Index) : CStatementSet
{
    public override async Task WriteAsync(StreamWriter writer)
    {
        // Length check
        await writer.WriteAsync("\tif (");
        await ArrayLengthField.WriteCodeStringAsync(writer);
        await writer.WriteAsync(" <= ");
        await Index.WriteCodeStringAsync(writer);
        await writer.WriteLineAsync(") {");
        
        // Error message
        await writer.WriteAsync("\t\tprintf(\"Attempted to access to ");
        await Array.WriteCodeStringAsync(writer);
        await writer.WriteAsync("[%d], but only %u items are in the array\", ");
        await Index.WriteCodeStringAsync(writer);
        await writer.WriteAsync(", ");
        await ArrayLengthField.WriteCodeStringAsync(writer);
        await writer.WriteLineAsync(");");
        await writer.WriteLineAsync("\t\tabort();");
        await writer.WriteLineAsync("\t}");
    }
}