using Dntc.Common.Syntax.Expressions;

namespace Dntc.Common.Syntax.Statements;

public record ArrayStoreStatementSet(
    FieldAccessExpression ArrayLengthField,
    FieldAccessExpression ArrayItemsField,
    CBaseExpression Array,
    DereferencedValueExpression Index,
    DereferencedValueExpression Value) : CStatementSet
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
        await writer.WriteAsync("\t\tprintf(\"ATtempted to write to ");
        await Array.WriteCodeStringAsync(writer);
        await writer.WriteAsync("[%ld], but only %zu items are in the array\", ");
        await Index.WriteCodeStringAsync(writer);
        await writer.WriteAsync(", ");
        await ArrayLengthField.WriteCodeStringAsync(writer);
        await writer.WriteLineAsync(");");
        await writer.WriteLineAsync("\t\tabort();");
        await writer.WriteLineAsync("\t}");
        
        // Assignment
        await writer.WriteAsync("\t");
        await ArrayItemsField.WriteCodeStringAsync(writer);
        await writer.WriteAsync("[");
        await Index.WriteCodeStringAsync(writer);
        await writer.WriteAsync("] = ");
        await Value.WriteCodeStringAsync(writer);
        await writer.WriteLineAsync(";");
    }
}