using Mono.Cecil;

namespace Dntc.Common.Definitions.Definers;

/// <summary>
/// Picks an implementation of different `I*Definer`s.
/// </summary>
public class DefinerDecider
{
    private readonly Dictionary<IlTypeName, IDotNetMethodDefiner> _methodDefiners = new();
    private readonly Dictionary<IlTypeName, IDotNetTypeDefiner> _typeDefiners = new();
    private readonly DefaultDotNetMethodDefiner _defaultMethodDefiner = new();
    private readonly DefaultTypeDefiner _defaultTypeDefiner = new();

    /// <summary>
    /// Adds a mapping for a specific decider to be used when the specified attribute is found
    /// on a method.
    /// </summary>
    public void AddMapping(Type attributeType, IDotNetMethodDefiner definer)
    {
        _methodDefiners.Add(new IlTypeName(attributeType.FullName!), definer);
    }

    /// <summary>
    /// Adds a mapping for a specific decider to be used when the specified attribute is found
    /// on a type.
    /// </summary>
    public void AddMapping(Type attributeType, IDotNetTypeDefiner definer)
    {
        _typeDefiners.Add(new IlTypeName(attributeType.FullName!), definer);
    }

    /// <summary>
    /// Analyzes the method's custom attributes and finds the first custom attribute whose type
    /// has been mapped to a definer. If none was found a default definer is returned.
    /// </summary>
    public IDotNetMethodDefiner GetDefiner(MethodDefinition method)
    {
        return GetDefinerInternal(method.FullName, method.CustomAttributes, _methodDefiners, _defaultMethodDefiner);
    }
    
    /// <summary>
    /// Analyzes the type's custom attributes and finds the first custom attribute whose type
    /// has been mapped to a definer. If none was found a default definer is returned.
    /// </summary>
    public IDotNetTypeDefiner GetDefiner(TypeDefinition type)
    {
        return GetDefinerInternal(type.FullName, type.CustomAttributes, _typeDefiners, _defaultTypeDefiner);
    }

    private static T GetDefinerInternal<T>(
        string name,
        IEnumerable<CustomAttribute> attributes, 
        Dictionary<IlTypeName, T> map,
        T defaultInstance)
    {
        var definers = attributes
            .Select(x => new IlTypeName(x.AttributeType.FullName))
            .Select(x => new { TypeName = x, Definer = map.GetValueOrDefault(x) })
            .Where(x => x.Definer != null)
            .ToArray();

        if (definers.Length <= 1)
        {
            return definers.Select(x => x.Definer).FirstOrDefault() ?? defaultInstance;
        }
        
        var attributeNames = definers.Select(x => x.TypeName.Value)
            .Aggregate((x, y) => $"{x}, {y}");

        var message = $"Type {name} has the following attributes on it which all map to different " +
                      $"definers: {attributeNames}. Each reference can only have a single attribute on it that's " +
                      $"tied to a definer.";

        throw new InvalidOperationException(message);

    }
}