namespace Dntc.Attributes;

/// <summary>
/// Allows specifying a string that should be used as the default value of a
/// global when transpiled.
///
/// This is used instead of relying on MSIL for now due to difficulty of accurately pulling
/// static assignments out of the static constructor. It's definitely not ideal.
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class InitialGlobalValueAttribute(string initialValue) : Attribute
{
    
} 
