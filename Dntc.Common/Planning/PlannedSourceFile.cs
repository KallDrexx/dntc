using Dntc.Common.Conversion;

namespace Dntc.Common.Planning;

public class PlannedSourceFile
{
    private readonly List<MethodConversionInfo> _implementedMethods = [];
    private readonly List<HeaderName> _referencedHeaders = [];
    
    public CSourceFileName Name { get; }

    public IReadOnlyList<MethodConversionInfo> ImplementedMethods => _implementedMethods;
    public IReadOnlyList<HeaderName> ReferencedHeaders => _referencedHeaders;

    public PlannedSourceFile(CSourceFileName name)
    {
        Name = name;
    }

    public void AddMethod(MethodConversionInfo method)
    {
        if (!_implementedMethods.Contains(method))
        {
            _implementedMethods.Add(method);
            AddReferencedHeader(method.Header);
        }
    }

    public void AddReferencedHeader(HeaderName headerName)
    {
        if (!_referencedHeaders.Contains(headerName))
        {
            _referencedHeaders.Add(headerName);
        }
    }
}