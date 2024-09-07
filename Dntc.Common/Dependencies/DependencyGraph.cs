using System.Text;

namespace Dntc.Common.Dependencies;

public class DependencyGraph
{
    public abstract record Node
    {
        public List<Node> Children { get; } = new();
    }

    public record TypeNode(ClrTypeName TypeName) : Node;

    public record MethodNode(ClrMethodId MethodId) : Node;
    
    public Node Root { get; private set; }

    public DependencyGraph(Catalog catalog, ClrMethodId rootMethod)
    {
        Root = CreateNode(catalog, rootMethod, new List<Node>());
    }

    private static Node CreateNode(Catalog catalog, ClrMethodId methodId, List<Node> path)
    {
        EnsureNotCircularReference(path, methodId);
        var method = catalog.FindMethod(methodId);
        if (method == null)
        {
            var message = $"No method in the catalog with the id '{methodId.Name}'";
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
            var typeNode = CreateNode(catalog, type, path);
            node.Children.Add(typeNode);
        }
        
        path.RemoveAt(path.Count - 1);
        return node;
    }

    private static Node CreateNode(Catalog catalog, ClrTypeName typeName, List<Node> path)
    {
        EnsureNotCircularReference(path, typeName);
        var type = catalog.FindType(typeName);
        if (type == null)
        {
            var message = $"No type in the catalog with the name '{typeName.Name}'";
            throw new InvalidOperationException(message);
        }

        var node = new TypeNode(typeName);
        path.Add(node);

        var subTypes = type.Fields.Select(x => x.Type).Distinct();
        foreach (var subType in subTypes)
        {
            var subNode = CreateNode(catalog, subType, path);
            node.Children.Add(subNode);
        }
        
        path.RemoveAt(path.Count - 1);
        return node;
    }

    private static void EnsureNotCircularReference(List<Node> path, ClrMethodId id)
    {
        var referenceFound = false;
        for (var x = 0; x < path.Count; x++)
        {
            if (path[x] is MethodNode node && node.MethodId == id)
            {
                referenceFound = true;
                break;
            }
        }

        if (referenceFound)
        {
            ThrowCircularReferenceException(path, id.Name);
        }
    }

    private static void EnsureNotCircularReference(List<Node> path, ClrTypeName typeName)
    {
        var referenceFound = false;
        for (var x = 0; x < path.Count; x++)
        {
            if (path[x] is TypeNode node && node.TypeName == typeName)
            {
                referenceFound = true;
                break;
            }
        }

        if (referenceFound)
        {
            ThrowCircularReferenceException(path, typeName.Name);
        }
    }

    private static void ThrowCircularReferenceException(List<Node> path, string finalName)
    {
        var pathString = new StringBuilder();
        for (var x = 0; x < path.Count; x++)
        {
            var node = path[x];
            switch (node)
            {
                case MethodNode methodNode:
                    pathString.Append($"{methodNode.MethodId.Name} --> ");
                    break;
                
                case TypeNode typeNode:
                    pathString.Append($"{typeNode.TypeName.Name} --> ");
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