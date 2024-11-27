using System.Text;
using Dntc.Common.Definitions;
using Dntc.Common.OpCodeHandling;
using Mono.Cecil.Rocks;

namespace Dntc.Common.Dependencies;

public class DependencyGraph
{
    public abstract record Node
    {
        public List<Node> Children { get; } = new();
    }

    public record TypeNode(IlTypeName TypeName) : Node;

    public record MethodNode(IlMethodId MethodId, bool IsStaticConstructor) : Node;

    public record GlobalNode(IlFieldId FieldId) : Node;
    
    public Node Root { get; private set; }

    public DependencyGraph(DefinitionCatalog definitionCatalog, IlMethodId rootMethod)
    {
        Root = CreateNode(definitionCatalog, rootMethod, []);
    }

    private static Node CreateNode(DefinitionCatalog definitionCatalog, GenericInvokedMethod invokedMethod, List<Node> path)
    {
        var invokedDefinition = definitionCatalog.Get(invokedMethod.MethodId);
        if (invokedDefinition == null)
        {
            // This generic with these specific type arguments is new, so we need to add a definition for it
            var sourceMethod = definitionCatalog.Get(invokedMethod.OriginalMethodId);
            if (sourceMethod == null)
            {
                var message = $"Generic method '{invokedMethod.MethodId}' refers to the original method " +
                              $"'{invokedMethod.OriginalMethodId}', but that method's definition isn't known";
                throw new InvalidOperationException(message);
            }

            // Clone the method for this particular use case
            if (sourceMethod is not DotNetDefinedMethod dotNetDefinedMethod)
            {
                var message = $"Generic method '{invokedMethod.MethodId}' refers to the original method " +
                              $"'{invokedMethod.OriginalMethodId}', but that method is not a dot net defined method, " +
                              $"but is instead a {sourceMethod.GetType().FullName}";
                throw new InvalidOperationException(message);
            }

            var newMethod = dotNetDefinedMethod.MakeGenericInstance(
                invokedMethod.MethodId, 
                invokedMethod.GenericArguments);
            
            definitionCatalog.Add([newMethod]);
        }

        return CreateNode(definitionCatalog, invokedMethod.MethodId, path);
    }

    private static Node CreateNode(
        DefinitionCatalog definitionCatalog, 
        IlMethodId methodId, 
        List<Node> path, 
        bool isStaticConstructor = false)
    {
        EnsureNotCircularReference(path, methodId);
        var method = definitionCatalog.Get(methodId);
        if (method == null)
        {
            var message = $"No method in the catalog with the id '{methodId.Value}'";
            throw new InvalidOperationException(message);
        }

        var node = new MethodNode(methodId, isStaticConstructor);
        path.Add(node);

        foreach (var type in method.GetReferencedTypes)
        {
            var typeNode = CreateNode(definitionCatalog, type, path);
            node.Children.Add(typeNode);
        }

        if (method is DotNetDefinedMethod dotNetDefinedMethod)
        {
            if (dotNetDefinedMethod.Definition.Body == null)
            {
                var message = $"Method call seen to '{methodId}', which is an abstract or interface method. Only " +
                              $"calls to concrete methods can be invoked";
                throw new InvalidOperationException(message);
            }

            foreach (var calledMethod in dotNetDefinedMethod.InvokedMethods)
            {
                Node methodNode;
                if (calledMethod is GenericInvokedMethod generic)
                {
                    methodNode = CreateNode(definitionCatalog, generic, path);
                }
                else
                {
                    methodNode = CreateNode(definitionCatalog, calledMethod.MethodId, path);
                }

                node.Children.Add(methodNode);
            }

            foreach (var type in dotNetDefinedMethod.ReferencedTypes)
            {
                var typeNode = CreateNode(definitionCatalog, type, path);
                node.Children.Add(typeNode);
            }

            foreach (var global in dotNetDefinedMethod.ReferencedGlobals)
            {
                var globalNode = CreateNode(definitionCatalog, global, path);
                node.Children.Add(globalNode);
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

        var subTypes = type.InstanceFields
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

    public static Node CreateNode(DefinitionCatalog definitionCatalog, IlFieldId fieldId, List<Node> path)
    {
        EnsureNotCircularReference(path, fieldId);
        var field = definitionCatalog.Get(fieldId);
        if (field == null)
        {
            var message = $"No global defined for the field '{fieldId}'";
            throw new InvalidOperationException(message);
        }

        var node = new GlobalNode(fieldId);
        path.Add(node);
        
        // If the declaring type has a static constructor, we need to depend on that. This will miss
        // any other types with static constructors that modify this static value, but there's not an
        // easy way to do that without analyzing *every* static constructor in the assembly.
        if (field is DotNetDefinedGlobal dotNetGlobal)
        {
            var staticConstructor = dotNetGlobal.Definition
                .DeclaringType
                .GetStaticConstructor();

            if (staticConstructor != null)
            {
                var newNode = CreateNode(definitionCatalog, new IlMethodId(staticConstructor.FullName), path);
                node.Children.Add(newNode);
            }
        }

        path.Remove(node);
        return node;
    }

    // TODO: Don't exception on circular references, just stop going down them.
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
    
    private static void EnsureNotCircularReference(List<Node> path, IlFieldId id)
    {
        var referenceFound = false;
        foreach (var node in path)
        {
            if (node is GlobalNode fieldNode && fieldNode.FieldId == id)
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