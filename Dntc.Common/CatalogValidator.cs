namespace Dntc.Common;

public static class CatalogValidator
{
    public interface IValidationError;

    public record MissingMethodDefinition(ClrMethodId MethodId) : IValidationError;

    public record MissingTypeDefinition(ClrTypeName TypeName) : IValidationError;

    public static IReadOnlyList<IValidationError> IsMethodImplementable(Catalog catalog, ClrMethodId id)
    {
        var method = catalog.FindMethod(id);
        if (method == null)
        {
            return new[] { new MissingMethodDefinition(id) };
        }

        var missingTypes = new HashSet<ClrTypeName>();
        if (catalog.FindType(method.ReturnType) == null)
        {
            missingTypes.Add(method.ReturnType);
        }

        foreach (var param in method.Parameters)
        {
            if (catalog.FindType(param.Type) == null)
            {
                missingTypes.Add(param.Type);
            }
        }

        foreach (var local in method.Locals)
        {
            if (catalog.FindType(local) == null)
            {
                missingTypes.Add(local);
            }
        }

        return missingTypes.OrderBy(x => x.Name)
            .Select(x => new MissingTypeDefinition(x))
            .ToArray();
    }
}