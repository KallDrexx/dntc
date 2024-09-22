using System.Text;
using Dntc.Common.Definitions;
using Dntc.Common.MethodAnalysis;
using Mono.Cecil;

namespace Dntc.Common.Dependencies;

public class DependencyGraph
{
    private readonly MethodAnalyzer _methodAnalyzer = new();
    
    public abstract record Node
    {
        public List<Node> Children { get; } = new();
    }

    public record TypeNode(IlTypeName TypeName) : Node;

    public record MethodNode(IlMethodId MethodId) : Node;
    
    public Node Root { get; private set; }

    public DependencyGraph(DefinitionCatalog definitionCatalog, IlMethodId rootMethod)
    {
        Root = CreateNode(definitionCatalog, rootMethod, new List<Node>());
    }

    private Node CreateNode(DefinitionCatalog definitionCatalog, IlMethodId methodId, List<Node> path)
    {
        EnsureNotCircularReference(path, methodId);
        var method = definitionCatalog.Get(methodId);
        if (method == null)
        {
            var message = $"No method in the catalog with the id '{methodId.Value}'";
            throw new InvalidOperationException(message);
        }

        var node = new MethodNode(methodId);
        path.Add(node);

        var allTypes = method.Parameters
            .Select(x => x.Type)
            .Concat(method.Locals)
            .Concat([method.ReturnType])
            .Distinct();

        foreach (var type in allTypes)
        {
            var typeNode = CreateNode(definitionCatalog, type, path);
            node.Children.Add(typeNode);
        }

        if (method is DotNetDefinedMethod dotNetDefinedMethod)
        {
            var analysisResults = _methodAnalyzer.Analyze(dotNetDefinedMethod);
            foreach (var calledMethod in analysisResults.CalledMethods)
            {
                var methodNode = CreateNode(definitionCatalog, calledMethod, path);
                node.Children.Add(methodNode);
            }
        }
        
        path.RemoveAt(path.Count - 1);
        return node;
    }

    private static Node CreateNode(DefinitionCatalog definitionCatalog, IlTypeName typeName, List<Node> path)
    {
        EnsureNotCircularReference(path, typeName);
        var type = definitionCatalog.Get(typeName);
        if (type == null)
        {
            var message = $"No type in the catalog with the name '{typeName.Value}'";
            throw new InvalidOperationException(message);
        }

        var node = new TypeNode(typeName);
        path.Add(node);

        var subTypes = type.Fields
            .Select(x => x.Type)
            .Concat(type.OtherReferencedTypes)
            .Distinct();
        
        foreach (var subType in subTypes)
        {
            var subNode = CreateNode(definitionCatalog, subType, path);
            node.Children.Add(subNode);
        }
        
        path.RemoveAt(path.Count - 1);
        return node;
    }

    private static void EnsureNotCircularReference(List<Node> path, IlMethodId id)
    {
        var referenceFound = false;
        foreach (var node in path)
        {
            if (node is MethodNode methodNode && methodNode.MethodId == id)
            {
                referenceFound = true;
                break;
            }
        }

        if (referenceFound)
        {
            ThrowCircularReferenceException(path, id.Value);
        }
    }

    private static void EnsureNotCircularReference(List<Node> path, IlTypeName typeName)
    {
        var referenceFound = false;
        foreach (var node in path)
        {
            if (node is TypeNode typeNode && typeNode.TypeName == typeName)
            {
                referenceFound = true;
                break;
            }
        }

        if (referenceFound)
        {
            ThrowCircularReferenceException(path, typeName.Value);
        }
    }

    private static void ThrowCircularReferenceException(List<Node> path, string finalName)
    {
        var pathString = new StringBuilder();
        foreach (var node in path)
        {
            switch (node)
            {
                case MethodNode methodNode:
                    pathString.Append($"{methodNode.MethodId.Value} --> ");
                    break;
                
                case TypeNode typeNode:
                    pathString.Append($"{typeNode.TypeName.Value} --> ");
                    break;
                
                default:
                    throw new NotSupportedException(node.GetType().FullName);
            }
        }

        pathString.Append(finalName);

        var message = $"Circular reference found: {pathString}";
        throw new InvalidOperationException(message);
    }
}