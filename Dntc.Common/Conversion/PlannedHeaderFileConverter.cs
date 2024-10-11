using Dntc.Common.Definitions;
using Dntc.Common.Planning;
using Dntc.Common.Syntax;
using Dntc.Common.Syntax.Statements;

namespace Dntc.Common.Conversion;

public class PlannedHeaderFileConverter
{
    private readonly ConversionCatalog _conversionCatalog;
    private readonly DefinitionCatalog _definitionCatalog;

    public PlannedHeaderFileConverter(ConversionCatalog conversionCatalog, DefinitionCatalog definitionCatalog)
    {
        _conversionCatalog = conversionCatalog;
        _definitionCatalog = definitionCatalog;
    }

    public HeaderFile Convert(PlannedHeaderFile plannedHeaderFile)
    {
        var guard = new HeaderGuard(plannedHeaderFile.Name);
        var includes = plannedHeaderFile.ReferencedHeaders
            .Select(x => new IncludeClause(x))
            .ToArray();

        var typeDeclarations = plannedHeaderFile.DeclaredTypes
            .Select(x => new { ConversionInfo = x, Definition = _definitionCatalog.Get(x.IlName) })
            .Select(x => new TypeDeclaration(x.ConversionInfo, x.Definition!, _conversionCatalog))
            .ToArray();

        var methodDeclarations = plannedHeaderFile.DeclaredMethods
            .Select(x => new { ConversionInfo = x, Definition = _definitionCatalog.Get(x.MethodId) })
            .Select(x => new MethodDeclaration(x.ConversionInfo, x.Definition!, _conversionCatalog))
            .ToArray();

        return new HeaderFile(guard, includes, typeDeclarations, methodDeclarations);
    }

    public SourceFile Convert(PlannedSourceFile plannedSourceFile)
    {
        var includes = plannedSourceFile.ReferencedHeaders
            .Select(x => new IncludeClause(x))
            .ToArray();

        var methodBlocks = plannedSourceFile.ImplementedMethods
            .Select(x =>
            {
                var definition = _definitionCatalog.Get(x.MethodId);
                var declaration = new MethodDeclaration(x, definition!, _conversionCatalog);
                return new MethodBlock(x, [], declaration);
            })
            .ToArray();

        return new SourceFile(includes, methodBlocks);
    }
}