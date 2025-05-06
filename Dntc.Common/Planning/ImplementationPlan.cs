using Dntc.Common.Conversion;
using Dntc.Common.Definitions;
using Dntc.Common.Definitions.CustomDefinedMethods;
using Dntc.Common.Dependencies;

namespace Dntc.Common.Planning;

/// <summary>
/// Describes all the files that are needed to perform a conversion.
/// </summary>
public class ImplementationPlan
{
    private readonly ConversionCatalog _conversionCatalog;
    private readonly DefinitionCatalog _definitionCatalog;
    private readonly Dictionary<HeaderName, PlannedHeaderFile> _headers = new();
    private readonly Dictionary<CSourceFileName, PlannedSourceFile> _sourceFiles = new();
    private bool _staticConstructorInitializerAdded = false;

    public IEnumerable<PlannedHeaderFile> Headers => _headers.Values;
    public IEnumerable<PlannedSourceFile> SourceFiles => _sourceFiles.Values;

    public ImplementationPlan(ConversionCatalog conversionCatalog, DefinitionCatalog definitionCatalog)
    {
        _conversionCatalog = conversionCatalog;
        _definitionCatalog = definitionCatalog;
    }

    public void AddMethodGraph(DependencyGraph graph)
    {
        ProcessNode(graph.Root);
    }

    private void AddReferencedHeaders(DependencyGraph.Node node, PlannedHeaderFile headerFile)
    {
        if (node.IsPredeclared)
        {
            return;
        }

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
                    if (childMethod.Header != null)
                    {
                        headerFile.AddReferencedHeader(childMethod.Header.Value);
                    }

                    break;

                case DependencyGraph.FieldNode globalNode:
                    var global = _conversionCatalog.Find(globalNode.FieldId);
                    if (global.Header != null)
                    {
                        headerFile.AddReferencedHeader(global.Header.Value);
                    }

                    var globalDefinition = _definitionCatalog.Get(globalNode.FieldId);
                    if (globalDefinition != null)
                    {
                        var type = _conversionCatalog.Find(globalDefinition.IlType);
                        if (type.Header != null)
                        {
                            headerFile.AddReferencedHeader(type.Header.Value);
                        }
                    }

                    foreach (var header in global.ReferencedHeaders)
                    {
                        headerFile.AddReferencedHeader(header);
                    }

                    break;

                default:
                    throw new NotSupportedException(child.GetType().FullName);
            }
        }
    }

    private void AddReferencedHeaders(DependencyGraph.Node node, PlannedSourceFile sourceFile)
    {
        if (node.IsPredeclared)
        {
            return;
        }

        foreach (var child in node.Children)
        {
            switch (child)
            {
                case DependencyGraph.TypeNode typeNode:
                    var childType = _conversionCatalog.Find(typeNode.TypeName);
                    if (childType.Header != null)
                    {
                        sourceFile.AddReferencedHeader(childType.Header.Value);
                    }

                    break;

                case DependencyGraph.MethodNode methodNode:
                    var childMethod = _conversionCatalog.Find(methodNode.MethodId);
                    if (childMethod.Header != null)
                    {
                        sourceFile.AddReferencedHeader(childMethod.Header.Value);
                    }

                    var methodDefinition = _definitionCatalog.Get(methodNode.MethodId);
                    if (methodDefinition != null)
                    {
                        foreach (var header in methodDefinition.ReferencedHeaders)
                        {
                            sourceFile.AddReferencedHeader(header);
                        }
                    }

                    break;

                case DependencyGraph.FieldNode globalNode:
                    var global = _conversionCatalog.Find(globalNode.FieldId);
                    if (global.Header != null)
                    {
                        sourceFile.AddReferencedHeader(global.Header.Value);
                    }

                    var globalDefinition = _definitionCatalog.Get(globalNode.FieldId);
                    if (globalDefinition != null)
                    {
                        var type = _conversionCatalog.Find(globalDefinition.IlType);
                        if (type.Header != null)
                        {
                            sourceFile.AddReferencedHeader(type.Header.Value);
                        }
                    }

                    foreach (var header in global.ReferencedHeaders)
                    {
                        sourceFile.AddReferencedHeader(header);
                    }

                    break;

                default:
                    throw new NotSupportedException(child.GetType().FullName);
            }
        }
    }

    private void ProcessNode(DependencyGraph.Node node)
    {
        if (node.IsPredeclared)
        {
            // Predeclared nodes should not have them or their children declared
            return;
        }

        if (node is not DependencyGraph.MethodNode)
        {
            foreach (var child in node.Children)
            {
                ProcessNode(child);
            }
        }

        if (node is DependencyGraph.TypeNode typeNode)
        {
            DeclareType(typeNode);
        }
        else if (node is DependencyGraph.MethodNode methodNode)
        {
            foreach (var child in node.Children)
            {
                if(child is DependencyGraph.MethodNode { IsOverride: true })
                    continue;
                
                ProcessNode(child);
            }
            DeclareMethod(methodNode);
            AddMethodImplementation(methodNode);

            if (methodNode.IsStaticConstructor)
            {
                AddStaticConstructorInitializer();
            }
            
            foreach (var child in node.Children)
            {
                if(child is DependencyGraph.MethodNode { IsOverride: true })
                    ProcessNode(child);
            }
        }
        else if (node is DependencyGraph.FieldNode fieldNode)
        {
            AddFieldDeclaration(fieldNode);
            AddFieldImplementation(fieldNode);
        }
        else
        {
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

        if (type.Header != null && type.SourceFileName != null)
        {
            var message = $"Type '{type.IlName}' has both a header and source file specified, but only " +
                          $"one should be set";

            throw new InvalidOperationException(message);
        }

        if (type.Header != null)
        {
            if (!_headers.TryGetValue(type.Header.Value, out var header))
            {
                header = new PlannedHeaderFile(type.Header.Value);
                _headers[header.Name] = header;
            }

            AddReferencedHeaders(node, header);

            var typeDefinition = _definitionCatalog.Get(node.TypeName.GetNonPointerOrRef());
            foreach (var referencedHeader in typeDefinition!.ManuallyReferencedHeaders)
            {
                header.AddReferencedHeader(referencedHeader);
            }

            header.AddDeclaredType(type);
        } 
        else if (type.SourceFileName != null)
        {
            if (!_sourceFiles.TryGetValue(type.SourceFileName.Value, out var sourceFile))
            {
                sourceFile = new PlannedSourceFile(type.SourceFileName.Value);
                _sourceFiles[sourceFile.Name] = sourceFile;
            }

            AddReferencedHeaders(node, sourceFile);

            var typeDefinition = _definitionCatalog.Get(node.TypeName);
            foreach (var referencedHeader in typeDefinition!.ManuallyReferencedHeaders)
            {
                sourceFile.AddReferencedHeader(referencedHeader);
            }

            sourceFile.AddDeclaredType(type);
        }
    }

    private void DeclareMethod(DependencyGraph.MethodNode node)
    {
        var method = _conversionCatalog.Find(node.MethodId);
        if (method.IsPredeclared || method.Header == null)
        {
            // We aren't declaring this method, so nothing to do here
            return;
        }

        if (!_headers.TryGetValue(method.Header!.Value, out var header))
        {
            header = new PlannedHeaderFile(method.Header.Value);
            _headers[method.Header.Value] = header;
        }

        AddReferencedHeaders(node, header);

        var methodDefinition = _definitionCatalog.Get(node.MethodId);
        foreach (var referencedHeader in methodDefinition!.ReferencedHeaders)
        {
            header.AddReferencedHeader(referencedHeader);
        }

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

        var methodDefinition = _definitionCatalog.Get(node.MethodId);
        foreach (var referencedHeader in methodDefinition!.ReferencedHeaders)
        {
            sourceFile.AddReferencedHeader(referencedHeader);
        }

        AddReferencedHeaders(node, sourceFile);
        sourceFile.AddMethod(method);
    }

    private void AddStaticConstructorInitializer()
    {
        if (_staticConstructorInitializerAdded)
        {
            return; // Already added
        }

        _staticConstructorInitializerAdded = true;
        var node = new DependencyGraph.MethodNode(StaticConstructorInitializerDefinedMethod.MethodId, false, false, false);
        ProcessNode(node);
    }

    private void AddFieldDeclaration(DependencyGraph.FieldNode node)
    {
        var field = _conversionCatalog.Find(node.FieldId);
        if (field.IsPredeclared || field.Header == null)
        {
            return;
        }

        if (!_headers.TryGetValue(field.Header.Value, out var header))
        {
            header = new PlannedHeaderFile(field.Header.Value);
            _headers[field.Header.Value] = header;
        }

        AddReferencedHeaders(node, header);

        // If a field isn't global, then its declaration will be part of its declaring
        // type's declaration
        if (node.IsGlobal)
        {
            header.AddDeclaredGlobal(field);
        }
    }

    private void AddFieldImplementation(DependencyGraph.FieldNode node)
    {
        var field = _conversionCatalog.Find(node.FieldId);
        if (field.IsPredeclared || field.SourceFileName == null)
        {
            return;
        }

        if (!_sourceFiles.TryGetValue(field.SourceFileName.Value, out var sourceFile))
        {
            sourceFile = new PlannedSourceFile(field.SourceFileName.Value);
            _sourceFiles[field.SourceFileName.Value] = sourceFile;
        }

        AddReferencedHeaders(node, sourceFile);

        // If a field isn't global, then its declaration will be part of its declaring
        // type's declaration
        if (node.IsGlobal)
        {
            sourceFile.AddImplementedGlobal(field);
        }
    }
}