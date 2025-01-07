namespace Dntc.Attributes;

/// <summary>
/// Specifies that the global this is attached to is a string that should not be
/// transpiled to `char *`, but instead `char name[]`.
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class NonPointerStringAttribute : Attribute
{
    
}