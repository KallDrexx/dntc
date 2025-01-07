namespace Dntc.Attributes;

/// <summary>
/// Declares that the attached object should be transpiled with the specified string in the
/// attribute position.
/// </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Field)]
public class WithCAttributeAttribute(string text) : Attribute
{
    
}