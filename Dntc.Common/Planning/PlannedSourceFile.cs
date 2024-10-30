using Dntc.Common.Conversion;

namespace Dntc.Common.Planning;

public class PlannedSourceFile
{
    private readonly List<MethodConversionInfo> _implementedMethods = [];
    private readonly List<HeaderName> _referencedHeaders = [];
    private readonly List<TypeConversionInfo> _typesWithGlobals = [];
    
    public CSourceFileName Name { get; }

    public IReadOnlyList<MethodConversionInfo> ImplementedMethods => _implementedMethods;
    public IReadOnlyList<HeaderName> ReferencedHeaders => _referencedHeaders;
    public IReadOnlyList<TypeConversionInfo> TypesWithGlobals => _typesWithGlobals;

    public PlannedSourceFile(CSourceFileName name)
    {
        Name = name;
    }

    public void AddMethod(MethodConversionInfo method)
    {
        if (!_implementedMethods.Contains(method))
        {
            _implementedMethods.Add(method);

            if (method.Header != null)
            {
                AddReferencedHeader(method.Header.Value);
            }
        }
    }

    public void AddReferencedHeader(HeaderName headerName)
    {
        if (!_referencedHeaders.Contains(headerName))
        {
            _referencedHeaders.Add(headerName);
        }
    }

    public void AddTypeWithStaticField(TypeConversionInfo type)
    {
        if (!_typesWithGlobals.Contains(type))
        {
            _typesWithGlobals.Add(type);
        }
    }
}