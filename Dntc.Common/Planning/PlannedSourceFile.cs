﻿using Dntc.Common.Conversion;

namespace Dntc.Common.Planning;

public class PlannedSourceFile
{
    private readonly List<MethodConversionInfo> _implementedMethods = [];
    private readonly List<HeaderName> _referencedHeaders = [];
    private readonly List<TypeConversionInfo> _typesWithGlobals = [];
    private readonly List<TypeConversionInfo> _declaredTypes = [];
    
    public CSourceFileName Name { get; }

    public IReadOnlyList<MethodConversionInfo> ImplementedMethods => _implementedMethods;
    public IReadOnlyList<HeaderName> ReferencedHeaders => _referencedHeaders;
    public IReadOnlyList<TypeConversionInfo> TypesWithGlobals => _typesWithGlobals;
    public IReadOnlyList<TypeConversionInfo> DeclaredTypes => _declaredTypes;

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
        var processedHeaderFiles = new HashSet<HeaderName>();
        var headersReferencedByOtherFiles = new HashSet<HeaderName>();

        foreach (var header in headers)
        {
            processedHeaderFiles.Add(header.Name);
            foreach (var referencedHeader in header.ReferencedHeaders)
            {
                headersReferencedByOtherFiles.Add(referencedHeader);
            }

            newSourceFile._declaredTypes.AddRange(header.DeclaredTypes);
            
            // We don't care about method declarations or global declarations, as they should have
            // implementations in the corresponding source files. It would be a bug if a header is
            // provided without its corresponding source file, but there's no easy way to enforce
            // that.
        }

        foreach (var sourceFile in sourceFiles)
        {
            foreach (var referencedHeader in sourceFile.ReferencedHeaders)
            {
                headersReferencedByOtherFiles.Add(referencedHeader);
            }

            newSourceFile._implementedMethods.AddRange(sourceFile.ImplementedMethods);
            newSourceFile._typesWithGlobals.AddRange(sourceFile.TypesWithGlobals);
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

    public void AddTypeWithStaticField(TypeConversionInfo type)
    {
        if (!_typesWithGlobals.Contains(type))
        {
            _typesWithGlobals.Add(type);
        }
    }
}