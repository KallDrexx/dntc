using Dntc.Common.Conversion;

namespace Dntc.Common.Planning;

public class PlannedSourceFile
{
    private readonly List<MethodConversionInfo> _implementedMethods = [];
    private readonly List<HeaderName> _referencedHeaders = [];
    private readonly List<TypeConversionInfo> _declaredTypes = [];
    private readonly List<FieldConversionInfo> _globals = [];
    private readonly List<MethodConversionInfo> _declaredMethods = [];

    public CSourceFileName Name { get; }

    public IReadOnlyList<MethodConversionInfo> ImplementedMethods => _implementedMethods;
    public IReadOnlyList<HeaderName> ReferencedHeaders => _referencedHeaders;
    public IReadOnlyList<TypeConversionInfo> DeclaredTypes => _declaredTypes;
    public IReadOnlyList<FieldConversionInfo> ImplementedGlobals => _globals;
    public IReadOnlyList<MethodConversionInfo> DeclaredMethods => _declaredMethods;

    public PlannedSourceFile(CSourceFileName name)
    {
        Name = name;
    }

    /// <summary>
    /// Creates a brand-new source file that consists of several header files
    /// and source files merged together.
    /// </summary>
    public static PlannedSourceFile CreateMerged(
        CSourceFileName name,
        IEnumerable<PlannedHeaderFile> headers,
        IEnumerable<PlannedSourceFile> sourceFiles)
    {
        var newSourceFile = new PlannedSourceFile(name);

        // Make sure headers are lists to maintain ordering
        var processedHeaderFiles = new List<HeaderName>();
        var headersReferencedByOtherFiles = new List<HeaderName>();

        foreach (var header in headers)
        {
            foreach (var referencedHeader in header.ReferencedHeaders)
            {
                if (!headersReferencedByOtherFiles.Contains(referencedHeader))
                {
                    headersReferencedByOtherFiles.Add(referencedHeader);
                }
            }

            processedHeaderFiles.Add(header.Name);

            newSourceFile._declaredTypes.AddRange(header.DeclaredTypes);

            // We don't care about method declarations or global declarations, as they should have
            // implementations in the corresponding source files. It would be a bug if a header is
            // provided without its corresponding source file, but there's no easy way to enforce
            // that.
            //
            // The exception to this is if the method only has a declaration without an implementation
            // (such as a macro) and thus must be in the merged output.
            foreach (var declaredMethod in header.DeclaredMethods.Where(x => x.IsDeclarationOnlyMethod))
            {
                newSourceFile._declaredMethods.Add(declaredMethod);
            }
        }

        foreach (var sourceFile in sourceFiles)
        {
            foreach (var referencedHeader in sourceFile.ReferencedHeaders)
            {
                if (!headersReferencedByOtherFiles.Contains(referencedHeader))
                {
                    headersReferencedByOtherFiles.Add(referencedHeader);
                }
            }

            newSourceFile._implementedMethods.AddRange(sourceFile.ImplementedMethods);
            newSourceFile._globals.AddRange(sourceFile._globals);
        }

        // Resolve which headers are actually required to be referenced (we shouldn't reference
        // any headers we merged in).
        newSourceFile._referencedHeaders.AddRange(
            headersReferencedByOtherFiles
                .Where(x => !processedHeaderFiles.Contains(x))
                .ToArray());

        return newSourceFile;
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

    public void AddImplementedGlobal(FieldConversionInfo field)
    {
        if (!_globals.Contains(field))
        {
            _globals.Add(field);
        }
    }

    public void AddDeclaredType(TypeConversionInfo type)
    {
        if (!_declaredTypes.Contains(type))
        {
            _declaredTypes.Add(type);
        }
    }
}