namespace Dntc.Attributes;

/// <summary>
/// Specifies that the method should have a custom name when transpiled
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class CustomFunctionNameAttribute(string nativeName) : Attribute
{
    public string NativeName => nativeName;
}