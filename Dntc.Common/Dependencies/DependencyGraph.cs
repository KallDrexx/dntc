using System.Text;
using Dntc.Common.Definitions;
using Dntc.Common.Definitions.CustomDefinedMethods;
using Dntc.Common.Definitions.ReferenceTypeSupport;
using Dntc.Common.OpCodeHandling;
using Mono.Cecil.Rocks;

namespace Dntc.Common.Dependencies;

public class DependencyGraph
{
    private readonly IMemoryManagementActions _memoryManagement;

    public abstract record Node(bool IsPredeclared)
    {
        public List<Node> Children { get; } = [];
    }

    public record TypeNode(IlTypeName TypeName, bool IsPredeclared) : Node(IsPredeclared);

    public record MethodNode(IlMethodId MethodId, bool IsStaticConstructor, bool IsPredeclared, bool IsOverride)
        : Node(IsPredeclared);

    public record FieldNode(IlFieldId FieldId, bool IsGlobal, bool IsPredeclared) : Node(IsPredeclared);
    
    public Node Root { get; private set; }

    public DependencyGraph(DefinitionCatalog definitionCatalog, IlMethodId rootMethod, IMemoryManagementActions memoryManagement)
    {
        _memoryManagement = memoryManagement;
        var firstNode = CreateNode(definitionCatalog, rootMethod, []);
        if (firstNode == null)
        {
            var message = $"Failed to create root node from '{rootMethod}'";
            throw new InvalidOperationException(message);
        }

        Root = firstNode;
    }

    public DependencyGraph(DefinitionCatalog definitionCatalog, IlFieldId global, IMemoryManagementActions memoryManagement)
    {
        _memoryManagement = memoryManagement;
        var node = CreateNode(definitionCatalog, global, []);
        if (node == null)
        {
            var message = $"Failed to create root node from '{global}'";
            throw new InvalidOperationException(message);
        }

        Root = node;
    }

    private MethodNode? CreateNode(DefinitionCatalog definitionCatalog, GenericInvokedMethod invokedMethod, List<Node> path)
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

        var newMethod = sourceMethod.MakeGenericInstance(invokedMethod.MethodId, invokedMethod.GenericArguments);
        definitionCatalog.Add([newMethod]);
        return CreateNode(definitionCatalog, invokedMethod.MethodId, path);
    }

    private MethodNode? CreateNode(DefinitionCatalog definitionCatalog, IlMethodId methodId, List<Node> path, bool isOverride = false)
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
        var isPredeclared = method is NativeDefinedMethod;
        
        var node = new MethodNode(methodId, isStaticConstructor, isPredeclared, isOverride);
        path.Add(node);

        foreach (var type in method.GetReferencedTypes)
        {
            var typeNode = CreateNode(definitionCatalog, type, path);
            if (typeNode != null)
            {
                node.Children.Add(typeNode);
            }
        }
        
        foreach (var calledMethod in method.InvokedMethods)
        {
            Node? methodNode;
            switch (calledMethod)
            {
                case GenericInvokedMethod generic:
                    methodNode = CreateNode(definitionCatalog, generic, path);
                    break;
                case CustomInvokedMethod custom:
                    if (definitionCatalog.Get(custom.MethodId) == null)
                    {
                        definitionCatalog.Add([custom.InvokedMethod]);
                    }
                        
                    methodNode = CreateNode(definitionCatalog, custom.MethodId, path);
                    break;
                default:
                    methodNode = CreateNode(definitionCatalog, calledMethod.MethodId, path);
                    break;
            }

            if (methodNode != null)
            {
                node.Children.Add(methodNode);
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

        if (method is DotNetDefinedMethod { Definition: { IsVirtual: true, IsNewSlot: true } })
        {
            var overrides = definitionCatalog.GetMethodOverrides(method);

            foreach (var derivedMethod in overrides)
            {
                var derivedNode = CreateNode(definitionCatalog, derivedMethod.Id, path, true);

                if (derivedNode != null)
                {
                    node.Children.Add(derivedNode);
                }
            }
        }

        path.RemoveAt(path.Count - 1);
        return node;
    }
    
    private TypeNode? CreateNode(DefinitionCatalog definitionCatalog, IlTypeName typeName, List<Node> path)
    {
        if (IsInPath(path, typeName))
        {
            if (IsTypeCircularReference(path, typeName))
            {
                var pathString = GetPathString(path.Concat([new TypeNode(typeName, false)]).ToArray());
                var message = $"A circular type dependency was found, but circular type references are not allowed. " +
                              $"Path is: \n{pathString}";

                throw new InvalidOperationException(message);
            }

            return null;
        }
        
        var type = definitionCatalog.Get(typeName);
        if (type == null)
        {
            // If this is a pointer variation of a type we already know about, use that as the definition
            type = definitionCatalog.Get(typeName.GetNonPointerOrRef());
            if (type == null)
            {
                var message = $"No type in the catalog with the name '{typeName.Value}'";
                throw new InvalidOperationException(message);
            }
        }

        var node = new TypeNode(typeName, type is NativeDefinedType);
        path.Add(node);

        foreach (var field in type.InstanceFields)
        {
            var fieldNode = CreateNode(definitionCatalog, field.Id, path);
            if (fieldNode != null)
            {
                node.Children.Add(fieldNode);
            }
        }

        foreach (var referencedType in type.OtherReferencedTypes)
        {
            var typeNode = CreateNode(definitionCatalog, referencedType, path);
            if (typeNode != null)
            {
                node.Children.Add(typeNode);
            }
        }

        // If this is a .net reference type, then we need to add both the prep to free and create methods to its dependency graph
        if (type is DotNetDefinedType dotNetType && !dotNetType.Definition.IsValueType)
        {
            // TODO: Reference types should automatically add the prepToFree method to their known required methods
            // or through some other mechanism. It shouldn't be up to the dependency graph to manually add them in.
            var prepMethod = definitionCatalog.Get(ReferenceTypeConstants.PrepTypeToFreeMethodId(type.IlName));
            if (prepMethod == null)
            {
                prepMethod = new PrepToFreeDefinedMethod(dotNetType, _memoryManagement);
                definitionCatalog.Add([prepMethod]);
            }

            var prepNode = CreateNode(definitionCatalog, prepMethod.Id, path);
            if (prepNode != null)
            {
                node.Children.Add(prepNode);
            }

            // Add the Create method alongside PrepForFree for all reference types
            var createMethodId = ReferenceTypeAllocationMethod.FormIlMethodId(dotNetType.Definition);
            var createMethod = definitionCatalog.Get(createMethodId);
            if (createMethod == null)
            {
                createMethod = new ReferenceTypeAllocationMethod(_memoryManagement, dotNetType.Definition);
                definitionCatalog.Add([createMethod]);
            }

            var createNode = CreateNode(definitionCatalog, createMethod.Id, path);
            if (createNode != null)
            {
                node.Children.Add(createNode);
            }
        }

        path.RemoveAt(path.Count - 1);
        return node;
    }

    private FieldNode? CreateNode(DefinitionCatalog definitionCatalog, IlFieldId fieldId, List<Node> path)
    {
        if (IsInPath(path, fieldId))
        {
            return null;
        }
        
        var field = definitionCatalog.Get(fieldId);
        if (field == null)
        {
            var message = $"No field defined for the '{fieldId}'";
            throw new InvalidOperationException(message);
        }

        var node = new FieldNode(fieldId, field.IsGlobal, field is NativeDefinedField);
        path.Add(node);

        var typeNode = CreateNode(definitionCatalog, field.IlType, path);
        if (typeNode != null)
        {
            node.Children.Add(typeNode);
        }
        
        // If the declaring type has a static constructor, we need to depend on that. This will miss
        // any other types with static constructors that modify this static value, but there's not an
        // easy way to do that without analyzing *every* static constructor in the assembly.
        //
        // TODO: Maybe it makes more sense just to add any static constructors as a dependency before
        // jumping to any new method or type. This is a bit easier now that we've converted the circular
        // dependency system to return null instead of throwing an exception.
        if (field is DotNetDefinedField dotNetGlobal)
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
            if (node is FieldNode globalNode && globalNode.FieldId == id)
            {
                return true;
            }
        }

        return false;
    }

    private static bool IsTypeCircularReference(List<Node> path, IlTypeName id)
    {
        // We have a circular reference only if the gap between the duplicates contains types
        // and fields. Method nodes don't count
        for (var x = path.Count - 1; x >= 0; x--)
        {
            if (path[x] is MethodNode)
            {
                return false;
            }

            if (path[x] is not TypeNode typeNode)
            {
                continue;
            }

            if (typeNode.TypeName == id)
            {
                return true;
            }
        }

        return false;
    }

    private static string GetPathString(IReadOnlyList<Node> path)
    {
        var builder = new StringBuilder();
        foreach (var node in path)
        {
            var nodeString = node switch
            {
                MethodNode methodNode => $"Method: {methodNode.MethodId.Value}",
                TypeNode typeNode => $"Type: {typeNode.TypeName.Value}",
                FieldNode fieldNode => $"Field: {fieldNode.FieldId.Value}",
                _ => throw new NotSupportedException(node.GetType().FullName),
            };

            builder.AppendLine($"\t{nodeString}");
        }

        return builder.ToString();
    }
}