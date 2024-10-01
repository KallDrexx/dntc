using Dntc.Common.Definitions;
using Dntc.Common.Dependencies;

namespace Dntc.Common.Conversion.Planning;

/// <summary>
/// Describes all the files that are needed to perform a conversion.
/// </summary>
public class ImplementationPlan
{
    private readonly ConversionCatalog _conversionCatalog;
    private readonly Dictionary<HeaderName, PlannedHeaderFile> _headers = new();
    private readonly Dictionary<CSourceFileName, PlannedSourceFile> _sourceFiles = new();

    public IEnumerable<PlannedHeaderFile> Headers => _headers.Values;
    public IEnumerable<PlannedSourceFile> SourceFiles => _sourceFiles.Values;

    public ImplementationPlan(ConversionCatalog conversionCatalog)
    {
        _conversionCatalog = conversionCatalog;
    }

    public void AddMethodGraph(DependencyGraph graph)
    {
        ProcessNode(graph.Root);
    }

    private void AddReferencedHeaders(DependencyGraph.Node node, PlannedHeaderFile headerFile)
    {
        foreach (var child in node.Children)
        {
            switch (child)
            {
                case DependencyGraph.TypeNode typeNode:
                    var childType = _conversionCatalog.Find(typeNode.TypeName);
                    if (childType.Header != null)
                    {
                        headerFile.AddReferencedHeader(childType.Header.Value);
                    }

                    foreach (var header in childType.ReferencedHeaders)
                    {
                        headerFile.AddReferencedHeader(header);
                    }
                    break;
                
                case DependencyGraph.MethodNode methodNode:
                    var childMethod = _conversionCatalog.Find(methodNode.MethodId);
                    headerFile.AddReferencedHeader(childMethod.Header);
                    break;
                
                default:
                    throw new NotSupportedException(child.GetType().FullName);
            }
        }
    }
    
    private void AddReferencedHeaders(DependencyGraph.Node node, PlannedSourceFile sourceFile)
    {
        foreach (var child in node.Children)
        {
            switch (child)
            {
                case DependencyGraph.TypeNode typeNode:
                    var childType = _conversionCatalog.Find(typeNode.TypeName);
                    if (childType.Header != null )
                    {
                        sourceFile.AddReferencedHeader(childType.Header.Value);
                    }
                    break;
                
                case DependencyGraph.MethodNode methodNode:
                    var childMethod = _conversionCatalog.Find(methodNode.MethodId);
                    sourceFile.AddReferencedHeader(childMethod.Header);
                    break;
                
                default:
                    throw new NotSupportedException(child.GetType().FullName);
            }
        }
    }

    private void ProcessNode(DependencyGraph.Node node)
    {
        foreach (var child in node.Children)
        {
            ProcessNode(child);
        }

        switch (node)
        {
            case DependencyGraph.TypeNode typeNode:
                DeclareType(typeNode);
                break;
            
            case DependencyGraph.MethodNode methodNode:
                DeclareMethod(methodNode);
                AddMethodImplementation(methodNode);
                break;
            
            default:
                throw new NotSupportedException(node.GetType().FullName);
        }
    }

    private void DeclareType(DependencyGraph.TypeNode node)
    {
        var type = _conversionCatalog.Find(node.TypeName);
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
        
        AddReferencedHeaders(node, header);
        header.AddDeclaredType(type);
    }

    private void DeclareMethod(DependencyGraph.MethodNode node)
    {
        var method = _conversionCatalog.Find(node.MethodId);
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
        
        AddReferencedHeaders(node, header);
        header.AddDeclaredMethod(method);
    }

    private void AddMethodImplementation(DependencyGraph.MethodNode node)
    {
        var method = _conversionCatalog.Find(node.MethodId);
        if (method.IsPredeclared || method.SourceFileName == null)
        {
            return;
        }

        if (!_sourceFiles.TryGetValue(method.SourceFileName.Value, out var sourceFile))
        {
            sourceFile = new PlannedSourceFile(method.SourceFileName.Value);
            _sourceFiles[sourceFile.Name] = sourceFile;
        }
        
        AddReferencedHeaders(node, sourceFile);
        sourceFile.AddMethod(method);
    }
}