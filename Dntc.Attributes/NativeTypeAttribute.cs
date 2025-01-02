namespace Dntc.Attributes;

/// <summary>
/// Specifies that the type should not be transpiled and references should use a native type instead.
/// </summary>
#pragma warning disable CS9113 // Parameter is unread.
[AttributeUsage(AttributeTargets.Struct)]
public class NativeTypeAttribute(string nativeTypeName, string? headerName) : Attribute
#pragma warning restore CS9113 // Parameter is unread.
{
}