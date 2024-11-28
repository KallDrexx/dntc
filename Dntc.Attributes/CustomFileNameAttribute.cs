namespace Dntc.Attributes;

/// <summary>
/// Specifies that the item the attribute is on should be transpiled to a specific header and
/// source file, instead of the ones that are computed for it.
/// </summary>
[AttributeUsage(
    AttributeTargets.Method | 
    AttributeTargets.Field | 
    AttributeTargets.Struct | 
    AttributeTargets.Property)]
public class CustomFileNameAttribute(string sourceFileName, string headerFileName) : Attribute
{
    public string SourceFileName => sourceFileName;
    public string HeaderFileName => headerFileName;
}