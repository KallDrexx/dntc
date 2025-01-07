namespace Dntc.Attributes;

/// <summary>
/// Specifies that the global it is attached to should be transpiled with a different name
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class CustomGlobalNameAttribute(string name) : Attribute
{
    
}