using Dntc.Common.Definitions;
using Dntc.Common.Dependencies;

namespace Dntc.Common.Conversion;

public class ConversionCatalog
{
    private readonly DefinitionCatalog _definitionCatalog;
    private readonly Dictionary<IlTypeName, TypeConversionInfo> _types = new();
    private readonly Dictionary<IlMethodId, MethodConversionInfo> _methods = new();

    public ConversionCatalog(DefinitionCatalog definitionCatalog)
    {
        _definitionCatalog = definitionCatalog;
    }

    public void Add(DependencyGraph dependencyGraph)
    {
        AddNode(dependencyGraph.Root);
    }

    public TypeConversionInfo Find(IlTypeName name)
    {
        if (_types.TryGetValue(name, out var info))
        {
            return info;
        }

        var message = $"Conversion catalog did not contain the type '{name.Value}'";
        throw new InvalidOperationException(message);
    }

    public MethodConversionInfo Find(IlMethodId method)
    {
        if (_methods.TryGetValue(method, out var info))
        {
            return info;
        }

        var message = $"Conversion catalog did not contain the type '{method.Value}'";
        throw new InvalidOperationException(message);
    }

    private void AddNode(DependencyGraph.Node node)
    {
        switch (node)
        {
            case DependencyGraph.TypeNode typeNode:
                AddNode(typeNode);
                break;
            
            case DependencyGraph.MethodNode methodNode:
                AddNode(methodNode);
                break;
            
            default:
                throw new NotSupportedException(node.GetType().FullName);
        }
    }

    private void AddNode(DependencyGraph.TypeNode node)
    {
        if (!_types.ContainsKey(node.TypeName))
        {
            var definition = _definitionCatalog.Get(node.TypeName);
            if (definition == null)
            {
                var message = $"Dependency graph contained node for type '{node.TypeName.Value}' but no " +
                              $"definition exists for it";
                throw new InvalidOperationException(message);
            }
            
            _types.Add(node.TypeName, new TypeConversionInfo(definition));
            AddChildren(node);
        }
    }

    private void AddNode(DependencyGraph.MethodNode node)
    {
        if (!_methods.ContainsKey(node.MethodId))
        {
            var definition = _definitionCatalog.Get(node.MethodId);
            if (definition == null)
            {
                var message = $"Dependency graph contained node for method '{node.MethodId.Value}' but no " +
                              $"definition exists for it";
                throw new InvalidOperationException(message);
            }
            
            _methods.Add(node.MethodId, new MethodConversionInfo(definition));
            AddChildren(node);
        }
    }

    private void AddChildren(DependencyGraph.Node node)
    {
        foreach (var child in node.Children)
        {
            AddNode(child);
        }
    }
}