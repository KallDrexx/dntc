using Mono.Cecil;

namespace Dntc.Common.Definitions.Definers;

/// <summary>
/// Specifies the order that definers will be tried before a default definer is used.
/// </summary>
public class DefinitionGenerationPipeline
{
    private readonly List<IDotNetFieldDefiner> _globalDefiners = new();
    private readonly List<IDotNetMethodDefiner> _methodDefiners = new();
    private readonly List<IDotNetTypeDefiner> _typeDefiners = new();

    public DefinitionGenerationPipeline()
    {
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
        _methodDefiners.Add(new DefaultDotNetMethodDefiner());
        _typeDefiners.Add(new DefaultTypeDefiner());
    }

    /// <summary>
    /// Adds the specified global definer to the end of the definer pipeline
    /// </summary>
    public void Add(IDotNetFieldDefiner definer)
    {
        _globalDefiners.Insert(_globalDefiners.Count - 1, definer);
    }
    
    /// <summary>
    /// Adds the specified method definer to the end of the definer pipeline
    /// </summary>
    public void Add(IDotNetMethodDefiner definer)
    {
        _methodDefiners.Insert(_methodDefiners.Count - 1, definer);
    }
    
    /// <summary>
    /// Adds the specified type definer to the end of the definer pipeline
    /// </summary>
    public void Add(IDotNetTypeDefiner definer)
    {
        _typeDefiners.Insert(_typeDefiners.Count - 1, definer);
    }

    public DefinedField Define(FieldDefinition field)
    {
        // Should always be able to get oen via default
        return _globalDefiners
            .Select(x => x.Define(field))
            .First(x => x != null)!;
    }

    public DefinedMethod Define(MethodDefinition method)
    {
        // Should always be able to get oen via default
        return _methodDefiners
            .Select(x => x.Define(method))
            .First(x => x != null)!;
    }

    public DefinedType Define(TypeDefinition type)
    {
        // Should always be able to get oen via default
        return _typeDefiners
            .Select(x => x.Define(type))
            .First(x => x != null)!;
    }
}