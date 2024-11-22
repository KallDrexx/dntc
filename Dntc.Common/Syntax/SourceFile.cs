using Dntc.Common.Syntax.Statements;

namespace Dntc.Common.Syntax;

public class SourceFile
{
    public IReadOnlyList<IncludeClause> IncludeClauses { get; }
    public IReadOnlyList<MethodBlock> Methods { get; }
    public IReadOnlyList<GlobalVariableDeclaration> Globals { get; }
    public IReadOnlyList<TypeDeclaration> TypeDeclarations { get; }
    
    public SourceFile(
        IReadOnlyList<IncludeClause> includeClauses, 
        IReadOnlyList<MethodBlock> methods, 
        IReadOnlyList<GlobalVariableDeclaration> globals,
        IReadOnlyList<TypeDeclaration> typeDeclarations)
    {
        IncludeClauses = includeClauses;
        Methods = methods;
        Globals = globals;
        TypeDeclarations = typeDeclarations;
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
        foreach (var type in TypeDeclarations)
        {
            await type.WriteAsync(writer);
        }

        await writer.WriteLineAsync();
        foreach (var global in Globals)
        {
            await global.WriteAsync(writer);
        }

        await writer.WriteLineAsync();
        foreach (var method in Methods)
        {
            await method.WriteAsync(writer);
            await writer.WriteLineAsync();
        }
    }
}