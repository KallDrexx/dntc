using Dntc.Common.Definitions;
using Dntc.Common.OpCodeHandling;
using Mono.Cecil.Rocks;

namespace Dntc.Common.Dependencies;

public class DependencyGraph
{
    public abstract record Node
    {
        public List<Node> Children { get; } = [];
    }

    public record TypeNode(IlTypeName TypeName) : Node;

    public record MethodNode(IlMethodId MethodId, bool IsStaticConstructor) : Node;

    public record GlobalNode(IlFieldId FieldId) : Node;
    
    public Node Root { get; private set; }

    public DependencyGraph(DefinitionCatalog definitionCatalog, IlMethodId rootMethod)
    {
        var firstNode = CreateNode(definitionCatalog, rootMethod, []);
        if (firstNode == null)
        {
            var message = $"Failed to create root node from '{rootMethod}'";
            throw new InvalidOperationException(message);
        }

        Root = firstNode;
    }

    public DependencyGraph(DefinitionCatalog definitionCatalog, IlFieldId global)
    {
        var node = CreateNode(definitionCatalog, global, []);
        if (node == null)
        {
            var message = $"Failed to create root node from '{global}'";
            throw new InvalidOperationException(message);
        }

        Root = node;
    }

    private static MethodNode? CreateNode(DefinitionCatalog definitionCatalog, GenericInvokedMethod invokedMethod, List<Node> path)
    {
        var invokedDefinition = definitionCatalog.Get(invokedMethod.MethodId);
        if (invokedDefinition != null)
        {
            return CreateNode(definitionCatalog, invokedMethod.MethodId, path);
        }
        
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

        return CreateNode(definitionCatalog, invokedMethod.MethodId, path);
    }

    private static MethodNode? CreateNode(DefinitionCatalog definitionCatalog, IlMethodId methodId, List<Node> path)
    {
        if (IsInPath(path, methodId))
        {
            return null;
        }
        
        var method = definitionCatalog.Get(methodId);
        if (method == null)
        {
            var message = $"No method in the catalog with the id '{methodId.Value}'";
            throw new InvalidOperationException(message);
        }

        var isStaticConstructor = method is DotNetDefinedMethod { Definition: { IsConstructor: true, IsStatic: true } };
        var node = new MethodNode(methodId, isStaticConstructor);
        path.Add(node);

        foreach (var type in method.GetReferencedTypes)
        {
            var typeNode = CreateNode(definitionCatalog, type, path);
            if (typeNode != null)
            {
                node.Children.Add(typeNode);
            }
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
                Node? methodNode;
                if (calledMethod is GenericInvokedMethod generic)
                {
                    methodNode = CreateNode(definitionCatalog, generic, path);
                }
                else
                {
                    methodNode = CreateNode(definitionCatalog, calledMethod.MethodId, path);
                }

                if (methodNode != null)
                {
                    node.Children.Add(methodNode);
                }
            }

            foreach (var type in dotNetDefinedMethod.ReferencedTypes)
            {
                var typeNode = CreateNode(definitionCatalog, type, path);
                if (typeNode != null)
                {
                    node.Children.Add(typeNode);
                }
            }

            foreach (var global in dotNetDefinedMethod.ReferencedGlobals)
            {
                var globalNode = CreateNode(definitionCatalog, global, path);
                if (globalNode != null)
                {
                    node.Children.Add(globalNode);
                }
            }
        }
        
        path.RemoveAt(path.Count - 1);
        return node;
    }
    
    private static TypeNode? CreateNode(DefinitionCatalog definitionCatalog, IlTypeName typeName, List<Node> path)
    {
        if (IsInPath(path, typeName))
        {
            return null;
        }
        
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
            if (subNode != null)
            {
                node.Children.Add(subNode);
            }
        }
        
        path.RemoveAt(path.Count - 1);
        return node;
    }

    private static GlobalNode? CreateNode(DefinitionCatalog definitionCatalog, IlFieldId fieldId, List<Node> path)
    {
        if (IsInPath(path, fieldId))
        {
            return null;
        }
        
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
        //
        // TODO: Maybe it makes more sense just to add any static constructors as a dependency before
        // jumping to any new method or type. This is a bit easier now that we've converted the circular
        // dependency system to return null instead of throwing an exception.
        if (field is DotNetDefinedGlobal dotNetGlobal)
        {
            var staticConstructor = dotNetGlobal.Definition
                .DeclaringType
                .GetStaticConstructor();

            if (staticConstructor != null)
            {
                var newNode = CreateNode(definitionCatalog, new IlMethodId(staticConstructor.FullName), path);
                if (newNode != null)
                {
                    node.Children.Add(newNode);
                }
            }
        }

        path.Remove(node);
        return node;
    }

    private static bool IsInPath(List<Node> path, IlMethodId id)
    {
        foreach (var node in path)
        {
            if (node is MethodNode methodNode && methodNode.MethodId == id)
            {
                return true;
            }
        }

        return false;
    }
    
    private static bool IsInPath(List<Node> path, IlTypeName id)
    {
        foreach (var node in path)
        {
            if (node is TypeNode typeNode && typeNode.TypeName == id)
            {
                return true;
            }
        }

        return false;
    }
    
    private static bool IsInPath(List<Node> path, IlFieldId id)
    {
        foreach (var node in path)
        {
            if (node is GlobalNode globalNode && globalNode.FieldId == id)
            {
                return true;
            }
        }

        return false;
    }
}