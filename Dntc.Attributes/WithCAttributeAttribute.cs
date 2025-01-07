namespace Dntc.Attributes;

/// <summary>
/// Declares that the attached object should be transpiled with the specified string in the
/// attribute position.
/// </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Field)]
#pragma warning disable CS9113 // Parameter is unread.
public class WithCAttributeAttribute(string text) : Attribute
#pragma warning restore CS9113 // Parameter is unread.
{
    
}