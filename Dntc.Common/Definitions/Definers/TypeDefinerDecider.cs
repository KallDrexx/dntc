using Mono.Cecil;

namespace Dntc.Common.Definitions.Definers;

/// <summary>
/// Picks a `DotNetTypeDefiner` implementation based on attributes on the type definition
/// </summary>
public class TypeDefinerDecider
{
    private readonly Dictionary<IlTypeName, IDotNetTypeDefiner> _definerMap = new();
    private readonly DefaultTypeDefiner _defaultTypeDefiner = new();

    /// <summary>
    /// Adds a mapping that if a type has the specified attribute on it, then it should use
    /// the provided definer to create the definition for it.
    /// </summary>
    public void AddMapping(IlTypeName attributeType, IDotNetTypeDefiner definer)
    {
        _definerMap.Add(attributeType, definer);
    }

    /// <summary>
    /// Gets the correct definer to create `DefinedType` instances for the specified
    /// Mono.cecil type definition. If the type has no attributes, or none of its
    /// attributes have been mapped to a definer, then a default definer will be returned.
    /// </summary>
    public IDotNetTypeDefiner GetDefiner(TypeDefinition type)
    {
        var definers = type.CustomAttributes
            .Select(x => new IlTypeName(x.AttributeType.FullName))
            .Select(x => new { TypeName = x, Definer = _definerMap.GetValueOrDefault(x) })
            .Where(x => x.Definer != null)
            .ToArray();

        if (definers.Length > 1)
        {
            var attributeNames = definers.Select(x => x.TypeName.Value)
                .Aggregate((x, y) => $"{x}, {y}");

            var message = $"Type {type.FullName} has the following attributes on it which all map to different " +
                          $"type definers: {attributeNames}. Each type can only have a single attribute on it that's " +
                          $"tied to a `IDotNetTypeDefiner'.";

            throw new InvalidOperationException(message);
        }

        return definers.Select(x => x.Definer).FirstOrDefault() ?? _defaultTypeDefiner;
    }
}