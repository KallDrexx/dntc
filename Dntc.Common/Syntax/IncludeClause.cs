namespace Dntc.Common.Syntax.Statements;

public record IncludeClause(HeaderName Header)
{
    public async Task WriteAsync(StreamWriter writer)
    {
        var headerName = Header.Value.StartsWith("<")
            ? Header.Value
            : $"\"{Header.Value}\"";
        
        await writer.WriteLineAsync($"#include {headerName}");
    }
}