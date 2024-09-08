namespace Dntc.Common.Conversion.Planning;

/// <summary>
/// Represents the plan of how a C header should be formed
/// </summary>
public class PlannedHeaderFile
{
    public HeaderName Name { get; }
    
    /// <summary>
    /// The headers this header should include
    /// </summary>
    public List<HeaderName> ReferencedHeaders { get; } = [];

    /// <summary>
    /// Types that should be directly declared in this header file. Declarations
    /// will occur in the order presented in this list.
    /// </summary>
    public List<TypeConversionInfo> DeclaredTypes { get; } = [];

    /// <summary>
    /// Methods that should be declared in this header. Declarations will occur
    /// in the order presented in this list.
    /// </summary>
    public List<MethodConversionInfo> DeclaredMethods { get; } = [];

    public PlannedHeaderFile(HeaderName name)
    {
        Name = name;
    }
}
