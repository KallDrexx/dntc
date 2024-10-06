using Dntc.Common.Syntax.Statements;

namespace Dntc.Common.Syntax;

public class SourceFile
{
    public IReadOnlyList<IncludeClause> IncludeClauses { get; }
    public IReadOnlyList<MethodBlock> Methods { get; }
    
    public SourceFile(IReadOnlyList<IncludeClause> includeClauses, IReadOnlyList<MethodBlock> methods)
    {
        IncludeClauses = includeClauses;
        Methods = methods;
    }

    public async Task WriteAsync(StreamWriter writer)
    {
        var orderedIncludes = IncludeClauses.OrderBy(x => !x.Header.Value.StartsWith('<'))
            .ThenBy(x => x.Header.Value);

        foreach (var include in orderedIncludes)
        {
            await include.WriteAsync(writer);
        }

        await writer.WriteLineAsync();

        foreach (var method in Methods)
        {
            await method.WriteAsync(writer);
            await writer.WriteLineAsync();
        }
    }
}