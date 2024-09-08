namespace Dntc.Common.Definitions;

public static class CatalogValidator
{
    public interface IValidationError;

    public record MissingMethodDefinition(IlMethodId MethodId) : IValidationError;

    public record MissingTypeDefinition(IlTypeName TypeName) : IValidationError;

    public static IReadOnlyList<IValidationError> IsMethodImplementable(DefinitionCatalog definitionCatalog, IlMethodId id)
    {
        var method = definitionCatalog.Find(id);
        if (method == null)
        {
            return new[] { new MissingMethodDefinition(id) };
        }

        var missingTypes = new HashSet<IlTypeName>();
        if (definitionCatalog.Find(method.ReturnType) == null)
        {
            missingTypes.Add(method.ReturnType);
        }

        foreach (var param in method.Parameters)
        {
            if (definitionCatalog.Find(param.Type) == null)
            {
                missingTypes.Add(param.Type);
            }
        }

        foreach (var local in method.Locals)
        {
            if (definitionCatalog.Find(local) == null)
            {
                missingTypes.Add(local);
            }
        }

        return missingTypes.OrderBy(x => x.Value)
            .Select(x => new MissingTypeDefinition(x))
            .ToArray();
    }
}