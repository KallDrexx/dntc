namespace Dntc.Attributes;

/// <summary>
/// Instructs the transpiler to not place the item's declaration in the corresponding
/// header file. Used to ensure a type, global, or function is file scoped and inaccessible
/// to other files.
///
/// This can't be inferred by C# modifiers as it's not a direct 1-to-1 comparison between
/// the two.
/// </summary>
[AttributeUsage(
    AttributeTargets.Field | 
    AttributeTargets.Struct | 
    AttributeTargets.Method)]
public class IgnoreInHeaderAttribute : Attribute
{
    
}