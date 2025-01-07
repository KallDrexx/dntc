namespace Dntc.Attributes;

/// <summary>
/// Specifies that a method call should not be directly transpiled, and any references should
/// call the natively defined function call. 
/// </summary>
/// <param name="functionName">The native name of the function</param>
/// <param name="headerName">Comma delimited list of headers the native function call requires</param>
[AttributeUsage(AttributeTargets.Method)]
public class NativeFunctionCallAttribute(string functionName, string? headerName) : Attribute
{
    public string FunctionName => functionName;
    public string? HeaderName => headerName;
}