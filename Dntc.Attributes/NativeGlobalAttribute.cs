namespace Dntc.Attributes;

/// <summary>
/// Specifies that references to the field this attribute is attached to should reference a native
/// global when transpiled.
/// </summary>
/// <param name="globalName">
/// The name of the global in C that will be referenced after transpilation process.
/// </param>
/// <param name="headerName">
/// The header that needs to be referenced to access the global
/// </param>
[AttributeUsage(AttributeTargets.Field)]
public class NativeGlobalAttribute(string globalName, string? headerName) : Attribute
{
    public string GlobalName => globalName;
    public string? HeaderName => headerName;
}