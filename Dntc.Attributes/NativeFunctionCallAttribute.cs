namespace Dntc.Attributes;

/// <summary>
/// Specifies that a method call should not be directly transpiled, and any references should
/// call the natively defined function call. 
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class NativeFunctionCallAttribute(string functionName, string? headerName) : Attribute
{
    public string FunctionName => functionName;
    public string? HeaderName => headerName;
}