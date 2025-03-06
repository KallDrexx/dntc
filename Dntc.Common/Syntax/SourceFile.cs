using Dntc.Common.Syntax.Statements;

namespace Dntc.Common.Syntax;

public class SourceFile
{
    public IReadOnlyList<IncludeClause> IncludeClauses { get; }
    public IReadOnlyList<MethodBlock> Methods { get; }
    public IReadOnlyList<FieldDeclaration> Globals { get; }
    public IReadOnlyList<TypeDeclaration> TypeDeclarations { get; }
    public IReadOnlyList<MethodDeclaration> MethodDeclarations { get; }
    
    public SourceFile(
        IReadOnlyList<IncludeClause> includeClauses, 
        IReadOnlyList<MethodBlock> methods, 
        IReadOnlyList<FieldDeclaration> globals,
        IReadOnlyList<TypeDeclaration> typeDeclarations,
        IReadOnlyList<MethodDeclaration> methodDeclarations)
    {
        IncludeClauses = includeClauses;
        Methods = methods;
        Globals = globals;
        TypeDeclarations = typeDeclarations;
        MethodDeclarations = methodDeclarations;
    }

    public async Task WriteAsync(StreamWriter writer)
    {
        // NOTE: Make sure to keep include ordering
        foreach (var include in IncludeClauses)
        {
            await include.WriteAsync(writer);
        }

        foreach (var type in TypeDeclarations)
        {
            await writer.WriteLineAsync();
            await type.WriteAsync(writer);
        }

        await writer.WriteLineAsync();
        foreach (var global in Globals)
        {
            await global.WriteAsync(writer);
        }

        foreach (var method in MethodDeclarations)
        {
            await writer.WriteLineAsync();
            await method.WriteAsync(writer);
        }

        foreach (var method in Methods)
        {
            await writer.WriteLineAsync();
            await method.WriteAsync(writer);
        }
    }
}