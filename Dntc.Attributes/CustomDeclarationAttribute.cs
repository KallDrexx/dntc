namespace Dntc.Attributes;

/// <summary>
/// Specifies that the method has a custom declaration instead of the one generated for it. This
/// is mostly required if the method needs to be defined via a macro. 
/// </summary>
/// <param name="fullDeclaration">
/// The full declaration for this method when transpiled. This may include return types and
/// all parameters. If the method this is attached to contains parameters, then the parameters
/// will probably need to be contained in the custom declaration to be present. 
/// </param>
/// <param name="referredBy">
/// If not null, the custom name that this method is called by.
/// </param>
/// <param name="headerName">
/// If not null, the specified header is required to be included.
/// </param>
[AttributeUsage(AttributeTargets.Method)]
public class CustomDeclarationAttribute(
    string fullDeclaration, 
    string? referredBy, 
    string? headerName) : Attribute
{
    public string FullDeclaration => fullDeclaration;
    public string? ReferredBy => referredBy;
    public string? HeaderName => headerName;
}
