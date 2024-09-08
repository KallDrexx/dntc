namespace Dntc.Common.Conversion.Planning;

/// <summary>
/// Represents the plan of how a C header should be formed
/// </summary>
public class PlannedHeaderFile
{
    private readonly List<HeaderName> _referencedHeaders = [];
    private readonly List<TypeConversionInfo> _declaredTypes = [];
    private readonly List<MethodConversionInfo> _declaredMethods = [];
    
    public HeaderName Name { get; }

    /// <summary>
    /// The headers this header should include
    /// </summary>
    public IReadOnlyList<HeaderName> ReferencedHeaders => _referencedHeaders;

    /// <summary>
    /// Types that should be directly declared in this header file. Declarations
    /// will occur in the order presented in this list.
    /// </summary>
    public IReadOnlyList<TypeConversionInfo> DeclaredTypes => _declaredTypes;

    /// <summary>
    /// Methods that should be declared in this header. Declarations will occur
    /// in the order presented in this list.
    /// </summary>
    public IReadOnlyList<MethodConversionInfo> DeclaredMethods => _declaredMethods;

    public PlannedHeaderFile(HeaderName name)
    {
        Name = name;
    }

    public void AddReferencedHeader(HeaderName headerName)
    {
        if (!_referencedHeaders.Contains(headerName))
        {
            _referencedHeaders.Add(headerName);
        }
    }

    public void AddDeclaredType(TypeConversionInfo type)
    {
        if (!_declaredTypes.Contains(type))
        {
            _declaredTypes.Add(type);
        }
    }

    public void AddDeclaredMethod(MethodConversionInfo method)
    {
        if (!_declaredMethods.Contains(method))
        {
            _declaredMethods.Add(method);
        }
    }
}
