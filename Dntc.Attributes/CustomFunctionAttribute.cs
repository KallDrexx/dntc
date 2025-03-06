namespace Dntc.Attributes;

/// <summary>
/// Allows defining that a method should be transpiled with the defined custom declaration and
/// implementation. This is useful when a method needs to be transpiled differently than the
/// C# version. It is also useful to convert a C# method call into a C macro.
///
/// If a declaration is specified without an implementation, then it is assumed it is a header only
/// function/macro block. This can be used either for a header implemented function, or a
/// #define macro.
/// </summary>
/// <param name="declaration">
/// THe C code for how the function is declared. It should only contain an implementation block if the
/// <paramref name="implementation" /> parameter is null
/// </param>
/// <param name="implementation">The C code that makes the implementation block of the function</param>
/// <param name="referredBy">The name that the function is referred to by callers</param>
[AttributeUsage(AttributeTargets.Method)]
public class CustomFunctionAttribute(string declaration, string? implementation, string referredBy) : Attribute
{

}