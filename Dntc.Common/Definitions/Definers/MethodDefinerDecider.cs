using Mono.Cecil;

namespace Dntc.Common.Definitions.Definers;

/// <summary>
/// Picks an implementation of `IDotNetMethodDefiner` based on attributes on the method.
/// </summary>
public class MethodDefinerDecider
{
    private readonly Dictionary<IlTypeName, IDotNetMethodDefiner> _definerMap = new();
    private readonly DefaultDotNetMethodDefiner _defaultDefiner = new();

    /// <summary>
    /// Adds a mapping for a specific decider to be used when the specified attribute is found
    /// on a method.
    /// </summary>
    public void AddMapping(IlTypeName attributeType, IDotNetMethodDefiner definer)
    {
        _definerMap.Add(attributeType, definer);
    }

    /// <summary>
    /// Analyzes the method's custom attributes and finds the first custom attribute whose type
    /// has been mapped to a definer. If none was found a default definer is returned.
    ///
    /// It is not deterministic on which definer will be used if the method has multiple attributes
    /// on it that are mapped to two different definers.
    /// </summary>
    public IDotNetMethodDefiner GetDefiner(MethodDefinition method)
    {
        foreach (var customAttribute in method.CustomAttributes)
        {
            if (_definerMap.TryGetValue(new IlTypeName(customAttribute.AttributeType.FullName), out var definer))
            {
                return definer;
            }
        }

        return _defaultDefiner;
    }
}