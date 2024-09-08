using Dntc.Common.Definitions;
using Dntc.Common.Dependencies;

namespace Dntc.Common.Conversion.Planning;

/// <summary>
/// Describes all the files that are needed to perform a conversion.
/// </summary>
public class ImplementationPlan
{
    private readonly Dictionary<HeaderName, PlannedHeaderFile> _headers = new();
    private readonly Dictionary<CSourceFileName, PlannedSourceFile> _sourceFiles = new();

    public IEnumerable<PlannedHeaderFile> Headers => _headers.Values;
    public IEnumerable<PlannedSourceFile> SourceFiles => _sourceFiles.Values;

    public ImplementationPlan(DefinitionCatalog definitionCatalog, DependencyGraph dependencies)
    {
        var conversionCatalog = new ConversionCatalog(definitionCatalog, dependencies);
        ProcessNode(conversionCatalog, dependencies.Root);
    }

    private static void AddReferencedHeaders(ConversionCatalog catalog, DependencyGraph.Node node, PlannedHeaderFile headerFile)
    {
        foreach (var child in node.Children)
        {
            switch (child)
            {
                case DependencyGraph.TypeNode typeNode:
                    var childType = catalog.Find(typeNode.TypeName);
                    if (childType.Header != null && !headerFile.ReferencedHeaders.Contains(childType.Header.Value))
                    {
                        headerFile.ReferencedHeaders.Add(childType.Header.Value);
                    }
                    break;
                
                case DependencyGraph.MethodNode methodNode:
                    var childMethod = catalog.Find(methodNode.MethodId);
                    if (!headerFile.ReferencedHeaders.Contains(childMethod.Header))
                    {
                        headerFile.ReferencedHeaders.Add(childMethod.Header);
                    }
                    break;
                
                default:
                    throw new NotSupportedException(child.GetType().FullName);
            }
        }
    }
    
    private static void AddReferencedHeaders(
        ConversionCatalog catalog, 
        DependencyGraph.Node node, 
        PlannedSourceFile sourceFile)
    {
        foreach (var child in node.Children)
        {
            switch (child)
            {
                case DependencyGraph.TypeNode typeNode:
                    var childType = catalog.Find(typeNode.TypeName);
                    if (childType.Header != null )
                    {
                        sourceFile.AddReferencedHeader(childType.Header.Value);
                    }
                    break;
                
                case DependencyGraph.MethodNode methodNode:
                    var childMethod = catalog.Find(methodNode.MethodId);
                    sourceFile.AddReferencedHeader(childMethod.Header);
                    break;
                
                default:
                    throw new NotSupportedException(child.GetType().FullName);
            }
        }
    }

    private void ProcessNode(ConversionCatalog catalog, DependencyGraph.Node node)
    {
        foreach (var child in node.Children)
        {
            ProcessNode(catalog, child);
        }

        switch (node)
        {
            case DependencyGraph.TypeNode typeNode:
                DeclareType(catalog, typeNode);
                break;
            
            case DependencyGraph.MethodNode methodNode:
                DeclareMethod(catalog, methodNode);
                AddMethodImplementation(catalog, methodNode);
                break;
            
            default:
                throw new NotSupportedException(node.GetType().FullName);
        }
    }

    private void DeclareType(ConversionCatalog catalog, DependencyGraph.TypeNode node)
    {
        var type = catalog.Find(node.TypeName);
        if (type.IsPredeclared)
        {
            // No need to touch predeclared types
            return;
        }

        if (type.Header == null)
        {
            var message = $"Type '{type.IlName}' is not predeclared but has no header";
            throw new InvalidOperationException(message);
        }

        if (!_headers.TryGetValue(type.Header.Value, out var header))
        {
            header = new PlannedHeaderFile(type.Header.Value);
            _headers[header.Name] = header;
        }
        
        AddReferencedHeaders(catalog, node, header);
        header.DeclaredTypes.Add(type);
    }

    private void DeclareMethod(ConversionCatalog catalog, DependencyGraph.MethodNode node)
    {
        var method = catalog.Find(node.MethodId);
        if (method.IsPredeclared)
        {
            // We aren't declaring this method, so nothing to do here
            return;
        }
        
        if (!_headers.TryGetValue(method.Header, out var header))
        {
            header = new PlannedHeaderFile(method.Header);
            _headers[method.Header] = header;
        }
        
        AddReferencedHeaders(catalog, node, header);
        header.DeclaredMethods.Add(method);
    }

    private void AddMethodImplementation(ConversionCatalog catalog, DependencyGraph.MethodNode node)
    {
        var method = catalog.Find(node.MethodId);
        if (method.IsPredeclared)
        {
            return;
        }

        if (method.SourceFileName == null)
        {
            var message = $"Method `{method.MethodId.Value}` is not predeclared but has no source file named";
            throw new InvalidOperationException(message);
        }

        if (!_sourceFiles.TryGetValue(method.SourceFileName.Value, out var sourceFile))
        {
            sourceFile = new PlannedSourceFile(method.SourceFileName.Value);
            _sourceFiles[sourceFile.Name] = sourceFile;
        }
        
        AddReferencedHeaders(catalog, node, sourceFile);
        sourceFile.AddMethod(method);
    }
}