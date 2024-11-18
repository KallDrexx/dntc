namespace Dntc.Attributes;

/// <summary>
/// Specifies that references to the field or property this attribute is attached to should instead
/// reference a native global when transpiled.
/// </summary>
/// <param name="globalName">
/// The name of the global in C that will be referenced after transpilation process.
/// </param>
/// <param name="headerName">
/// The header that needs to be referenced to access the global
/// </param>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class NativeGlobalOnTranspileAttribute(string globalName, string? headerName) : Attribute
{
    public string GlobalName => globalName;
    public string? HeaderName => headerName;
}