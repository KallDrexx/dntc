namespace Dntc.Attributes;

/// <summary>
/// Specifies that the field this is attached to (if an array) is a statically
/// sized array with a size known at compile time, instead of a dynamically
/// created array. Can be attached to strings for a `char[]` value.
/// </summary>
/// <param name="numberOfElements">How many elements the array contains</param>
/// <param name="bypassBoundsCheck">If true, no bounds checks will be included when array members are accessed</param>
[AttributeUsage(AttributeTargets.Field)]
public class StaticallySizedArrayAttribute(int numberOfElements, bool bypassBoundsCheck = false) : Attribute
{
    
}