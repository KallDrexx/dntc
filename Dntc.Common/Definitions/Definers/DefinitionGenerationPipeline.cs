using Dntc.Common.Definitions.Mutators;
using Dntc.Common.Definitions.ReferenceTypeSupport;
using Mono.Cecil;

namespace Dntc.Common.Definitions.Definers;

/// <summary>
/// Specifies the order that definers will be tried before a default definer is used.
/// </summary>
public class DefinitionGenerationPipeline
{
    private readonly List<IDotNetFieldDefiner> _globalDefiners = [];
    private readonly List<IDotNetMethodDefiner> _methodDefiners = [];
    private readonly List<IDotNetTypeDefiner> _typeDefiners = [];
    private readonly List<IFieldDefinitionMutator> _fieldMutators = [];
    private readonly List<IMethodDefinitionMutator> _methodMutators = [];
    private readonly IMemoryManagementActions _memoryManagement;

    public DefinitionGenerationPipeline(IMemoryManagementActions memoryManagement)
    {
        _memoryManagement = memoryManagement;
        Reset();
    }

    /// <summary>
    /// Removes all added definers from the pipeline.
    /// </summary>
    public void Reset()
    {
        _globalDefiners.Clear();
        _methodDefiners.Clear();
        _typeDefiners.Clear();
        
        _globalDefiners.Add(new DefaultFieldDefiner());
        _methodDefiners.Add(new DefaultDotNetMethodDefiner(_memoryManagement));
        _typeDefiners.Add(new DefaultTypeDefiner());
    }

    /// <summary>
    /// Adds the specified global definer to the end of the definer pipeline
    /// </summary>
    public void Append(IDotNetFieldDefiner definer)
    {
        _globalDefiners.Insert(_globalDefiners.Count - 1, definer);
    }
    
    /// <summary>
    /// Adds the specified method definer to the end of the definer pipeline
    /// </summary>
    public void Append(IDotNetMethodDefiner definer)
    {
        _methodDefiners.Insert(_methodDefiners.Count - 1, definer);
    }
    
    /// <summary>
    /// Adds the specified type definer to the end of the definer pipeline
    /// </summary>
    public void Append(IDotNetTypeDefiner definer)
    {
        _typeDefiners.Insert(_typeDefiners.Count - 1, definer);
    }

    /// <summary>
    /// Adds the specified global definer to the beginning of the definer pipeline
    /// </summary>
    public void Prepend(IDotNetFieldDefiner definer)
    {
        _globalDefiners.Insert(0, definer);
    }

    /// <summary>
    /// Adds the specified method definer to the beginning of the definer pipeline
    /// </summary>
    public void Prepend(IDotNetMethodDefiner definer)
    {
        _methodDefiners.Insert(0, definer);
    }

    /// <summary>
    /// Adds the specified type definer to the beginning of the definer pipeline
    /// </summary>
    public void Prepend(IDotNetTypeDefiner definer)
    {
        _typeDefiners.Insert(0, definer);
    }

    /// <summary>
    /// Adds the specified field mutator to the end of the definer pipeline
    /// </summary>
    public void AppendFieldMutator(IFieldDefinitionMutator mutator)
    {
        _fieldMutators.Add(mutator);
    }

    /// <summary>
    /// Adds the specified field mutator to the beginning of the definer pipeline
    /// </summary>
    public void PrependFieldMutator(IFieldDefinitionMutator mutator)
    {
        _fieldMutators.Insert(0, mutator);
    }

    /// <summary>
    /// Adds the specified method mutator to the end of the definer pipeline
    /// </summary>
    public void AppendMethodMutator(IMethodDefinitionMutator mutator)
    {
        _methodMutators.Add(mutator);
    }

    /// <summary>
    /// Adds the specified method mutator to the beginning of the definer pipeline
    /// </summary>
    public void PrependMethodMutator(IMethodDefinitionMutator mutator)
    {
        _methodMutators.Insert(0, mutator);
    }

    public DefinedField Define(FieldDefinition field)
    {
        // Should always be able to get one via default
        var definition = _globalDefiners
            .Select(x => x.Define(field))
            .First(x => x != null)!;

        foreach (var mutator in _fieldMutators)
        {
            mutator.Mutate(definition, field);
        }

        return definition;
    }

    public DefinedMethod Define(MethodDefinition method)
    {
        // Should always be able to get one via default
        var definition = _methodDefiners
            .Select(x => x.Define(method))
            .First(x => x != null)!;

        foreach (var mutator in _methodMutators)
        {
            mutator.Mutate(definition, method);
        }

        return definition;
    }

    public DefinedType Define(TypeDefinition type)
    {
        // Should always be able to get one via default
        return _typeDefiners
            .Select(x => x.Define(type))
            .First(x => x != null)!;
    }
}