namespace Dntc.Attributes;

/// <summary>
/// Specifies that the type should not be transpiled and references should use a native type instead.
/// </summary>
[AttributeUsage(AttributeTargets.Struct)]
public class NativeTypeAttribute(string nativeTypeName, string? headerName) : Attribute
{
}