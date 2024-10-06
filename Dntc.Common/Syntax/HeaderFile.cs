using Dntc.Common.Syntax.Statements;

namespace Dntc.Common.Syntax;

public class HeaderFile
{
    private readonly HeaderGuard Guard;
    private readonly IReadOnlyList<IncludeClause> IncludeClauses;
    private readonly IReadOnlyList<TypeDeclaration> Types;
    private readonly IReadOnlyList<MethodDeclaration> Methods;
    
    public HeaderFile(
        HeaderGuard guard, 
        IReadOnlyList<IncludeClause> includeClauses, 
        IReadOnlyList<TypeDeclaration> types, 
        IReadOnlyList<MethodDeclaration> methods)
    {
        Guard = guard;
        IncludeClauses = includeClauses;
        Types = types;
        Methods = methods;
    }

    public async Task WriteAsync(StreamWriter writer)
    {
        await Guard.WriteStart(writer);
        await writer.WriteLineAsync();

        var orderedIncludes = IncludeClauses.OrderBy(x => !x.Header.Value.StartsWith('<'))
            .ThenBy(x => x.Header.Value);

        foreach (var include in orderedIncludes)
        {
            await include.WriteAsync(writer);
        }

        await writer.WriteLineAsync();
        foreach (var type in Types)
        {
            await type.WriteAsync(writer);
            await writer.WriteLineAsync();
        }

        foreach (var method in Methods)
        {
            await method.WriteAsync(writer);
            await writer.WriteLineAsync();
        }
    }
}