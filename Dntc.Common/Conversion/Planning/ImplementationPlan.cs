using Dntc.Common.Definitions;
using Dntc.Common.Dependencies;

namespace Dntc.Common.Conversion.Planning;

/// <summary>
/// Describes all the files that are needed to perform a conversion.
/// </summary>
public class ImplementationPlan
{
    private readonly Dictionary<HeaderName, PlannedHeader> _headers = new();

    public IEnumerable<PlannedHeader> Headers => _headers.Values;

    public ImplementationPlan(DefinitionCatalog definitionCatalog, DependencyGraph dependencies)
    {
        var conversionCatalog = new ConversionCatalog(definitionCatalog, dependencies);
        ProcessNode(conversionCatalog, dependencies.Root);
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
            header = new PlannedHeader(type.Header.Value);
            _headers[type.Header.Value] = header;
        }
        
        header.DeclaredTypes.Add(type);
        AddReferencedHeaders(catalog, node, header);
        
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
            header = new PlannedHeader(method.Header);
            _headers[method.Header] = header;
        }
        
        header.DeclaredMethods.Add(method);
        AddReferencedHeaders(catalog, node, header);
    }

    private void AddReferencedHeaders(ConversionCatalog catalog, DependencyGraph.Node node, PlannedHeader header)
    {
        foreach (var child in node.Children)
        {
            switch (child)
            {
                case DependencyGraph.TypeNode typeNode:
                    var childType = catalog.Find(typeNode.TypeName);
                    if (childType.Header != null && !header.ReferencedHeaders.Contains(childType.Header.Value))
                    {
                        header.ReferencedHeaders.Add(childType.Header.Value);
                    }
                    break;
                
                case DependencyGraph.MethodNode methodNode:
                    var childMethod = catalog.Find(methodNode.MethodId);
                    if (!header.ReferencedHeaders.Contains(childMethod.Header))
                    {
                        header.ReferencedHeaders.Add(childMethod.Header);
                    }
                    break;
                
                default:
                    throw new NotSupportedException(child.GetType().FullName);
            }
        }
    }
}