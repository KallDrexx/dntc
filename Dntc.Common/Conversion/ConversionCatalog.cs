using Dntc.Common.Definitions;
using Dntc.Common.Dependencies;

namespace Dntc.Common.Conversion;

public class ConversionCatalog
{
    private readonly Dictionary<IlTypeName, TypeConversionInfo> _types = new();
    private readonly Dictionary<IlMethodId, MethodConversionInfo> _methods = new();

    public ConversionCatalog(DefinitionCatalog definitionCatalog, DependencyGraph dependencies)
    {
        AddNode(definitionCatalog, dependencies.Root);
    }

    public TypeConversionInfo Find(IlTypeName name)
    {
        if (_types.TryGetValue(name, out var info))
        {
            return info;
        }

        var message = $"Conversion catalog did not contain the type '{name.Value}";
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

    private void AddNode(DefinitionCatalog definitionCatalog, DependencyGraph.Node node)
    {
        switch (node)
        {
            case DependencyGraph.TypeNode typeNode:
                AddNode(definitionCatalog, typeNode);
                break;
            
            case DependencyGraph.MethodNode methodNode:
                AddNode(definitionCatalog, methodNode);
                break;
            
            default:
                throw new NotSupportedException(node.GetType().FullName);
        }
    }

    private void AddNode(DefinitionCatalog definitionCatalog, DependencyGraph.TypeNode node)
    {
        if (!_types.ContainsKey(node.TypeName))
        {
            var definition = definitionCatalog.Find(node.TypeName);
            if (definition == null)
            {
                var message = $"Dependency graph contained node for type '{node.TypeName.Value}' but no " +
                              $"definition exists for it";
                throw new InvalidOperationException(message);
            }
            
            _types.Add(node.TypeName, new TypeConversionInfo(definition));
            AddChildren(definitionCatalog, node);
        }
    }

    private void AddNode(DefinitionCatalog definitionCatalog, DependencyGraph.MethodNode node)
    {
        if (!_methods.ContainsKey(node.MethodId))
        {
            var definition = definitionCatalog.Find(node.MethodId);
            if (definition == null)
            {
                var message = $"Dependency graph contained node for method '{node.MethodId.Value}' but no " +
                              $"definition exists for it";
                throw new InvalidOperationException(message);
            }
            
            _methods.Add(node.MethodId, new MethodConversionInfo(definition));
            AddChildren(definitionCatalog, node);
        }
    }

    private void AddChildren(DefinitionCatalog definitionCatalog, DependencyGraph.Node node)
    {
        foreach (var child in node.Children)
        {
            AddNode(definitionCatalog, child);
        }
    }
}