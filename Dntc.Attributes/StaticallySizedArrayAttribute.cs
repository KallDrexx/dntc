namespace Dntc.Attributes;

/// <summary>
/// Specifies that the field this is attached to (if an array) is a statically
/// sized array with a size known at compile time, instead of a dynamically
/// created array. Can be attached to strings for a `char[]` value.
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class StaticallySizedArrayAttribute(int numberOfElements) : Attribute
{
    
}