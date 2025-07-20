using System.Diagnostics.CodeAnalysis;
using Dntc.Common.Definitions;
using Dntc.Common.Definitions.CustomDefinedMethods;
using Dntc.Common.Dependencies;

namespace Dntc.Common.Conversion;

public class ConversionCatalog
{
    private readonly DefinitionCatalog _definitionCatalog;
    private readonly ConversionInfoCreator _conversionInfoCreator;
    private readonly Dictionary<IlTypeName, TypeConversionInfo> _types = new();
    private readonly Dictionary<IlMethodId, MethodConversionInfo> _methods = new();
    private readonly Dictionary<IlFieldId, FieldConversionInfo> _fields = new();

    public ConversionCatalog(DefinitionCatalog definitionCatalog, ConversionInfoCreator conversionInfoCreator)
    {
        _definitionCatalog = definitionCatalog;
        _conversionInfoCreator = conversionInfoCreator;
    }

    public void Add(DependencyGraph dependencyGraph)
    {
        AddNode(dependencyGraph.Root);
    }

    public void Add(IlTypeName typeName)
    {
        if (_types.ContainsKey(typeName))
        {
            return;
        }
        
        AddNode(new DependencyGraph.TypeNode(typeName, false));
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

    public bool TryFind(IlMethodId name, [MaybeNullWhen(false)] out MethodConversionInfo info)
    {
        return _methods.TryGetValue(name, out info);
    }

    public bool TryFind(IlTypeName name, [MaybeNullWhen(false)] out TypeConversionInfo info)
    {
        return _types.TryGetValue(name, out info);
    }

    public FieldConversionInfo Find(IlFieldId fieldId)
    {
        if (_fields.TryGetValue(fieldId, out var info))
        {
            return info;
        }

        var message = $"Conversion catalog did not have did not contain a global for the field '{fieldId}'";
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
            
            case DependencyGraph.FieldNode globalNode:
                AddNode(globalNode);
                break;
            
            default:
                throw new NotSupportedException(node.GetType().FullName);
        }
    }

    private void AddNode(DependencyGraph.TypeNode node)
    {
        if (!_types.ContainsKey(node.TypeName))
        {
            var definition = _definitionCatalog.Get(node.TypeName.GetNonPointerOrRef());
            if (definition == null)
            {
                var message = $"Dependency graph contained node for type '{node.TypeName.Value}' but no " +
                              $"definition exists for it";
                throw new InvalidOperationException(message);
            }
            
            _types.Add(node.TypeName, _conversionInfoCreator.Create(definition, node.TypeName.IsPointer()));
            AddChildren(node);
        }
    }

    private void AddNode(DependencyGraph.FieldNode node)
    {
        if (_fields.ContainsKey(node.FieldId))
        {
            return;
        }

        var definition = _definitionCatalog.Get(node.FieldId);
        if (definition == null)
        {
            var message = $"Dependency graph contained a node for field `{node.FieldId}` but no " +
                          $"definition exists for it";
            throw new InvalidOperationException(message);
        }

        AddChildren(node);

        var fieldType = Find(definition.IlType);
        _fields.Add(node.FieldId, _conversionInfoCreator.Create(definition, fieldType));
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
            
            AddChildren(node);
            var conversionInfo = _conversionInfoCreator.Create(definition, this);
            _methods.Add(node.MethodId, conversionInfo);

            if (node.IsStaticConstructor)
            {
                var initializerDefinition =
                    _definitionCatalog.Get(StaticConstructorInitializerDefinedMethod.MethodId);

                if (initializerDefinition == null)
                {
                    var message = "No static constructor initializer definition found.";
                    throw new InvalidOperationException(message);
                }

                if (initializerDefinition is not StaticConstructorInitializerDefinedMethod initMethodDefinition)
                {
                    var message = $"Unexpected type of static constructor initialization method " +
                                  $"of {initializerDefinition.GetType().FullName}";

                    throw new InvalidOperationException(message);
                }

                _methods.TryAdd(
                    initializerDefinition.Id,
                    _conversionInfoCreator.Create(initializerDefinition, this));
                
                initMethodDefinition.AddStaticConstructor(conversionInfo);
            }
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