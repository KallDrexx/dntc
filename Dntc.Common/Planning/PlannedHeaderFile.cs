using Dntc.Common.Conversion;

namespace Dntc.Common.Planning;

/// <summary>
/// Represents the plan of how a C header should be formed
/// </summary>
public class PlannedHeaderFile
{
    private readonly List<HeaderName> _referencedHeaders = [];
    private readonly List<TypeConversionInfo> _declaredTypes = [];
    private readonly List<MethodConversionInfo> _declaredMethods = [];
    private readonly List<FieldConversionInfo> _declaredGlobals = [];
    
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

    /// <summary>
    /// Globals that are declared in this header.
    /// </summary>
    public IReadOnlyList<FieldConversionInfo> DeclaredGlobals => _declaredGlobals;

    public PlannedHeaderFile(HeaderName name)
    {
        Name = name;
    }

    public void AddReferencedHeader(HeaderName headerName)
    {
        if (headerName != Name && !_referencedHeaders.Contains(headerName))
        {
            _referencedHeaders.Add(headerName);
        }
    }

    public void AddDeclaredType(TypeConversionInfo type)
    {
        // We might have two instances of `TypeConversionInfo` if one was created from a
        // pointer or reference parameter/return type instead of a stand-alone object. Therefore,
        // we need to compare the il names instead of relying on `.Contains()`.
        if (_declaredTypes.All(x => x.IlName != type.IlName))
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

    public void AddDeclaredGlobal(FieldConversionInfo field)
    {
        if (!_declaredGlobals.Contains(field))
        {
            _declaredGlobals.Add(field);
        }
    }
}
