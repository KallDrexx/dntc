namespace Dntc.Attributes;

/// <summary>
/// Specifies that the global it is attached to should be transpiled with a different name
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
#pragma warning disable CS9113 // Parameter is unread.
public class CustomGlobalNameAttribute(string name) : Attribute
#pragma warning restore CS9113 // Parameter is unread.
{
    
}