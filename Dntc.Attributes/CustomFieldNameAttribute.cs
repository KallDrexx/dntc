namespace Dntc.Attributes;

/// <summary>
/// Specifies that the field it is attached to should be transpiled with a different name
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class CustomFieldNameAttribute(string name) : Attribute
{
    
}