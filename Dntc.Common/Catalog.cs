using Mono.Cecil;

namespace Dntc.Common;

public class Catalog
{
    private readonly Dictionary<ClrTypeName, TypeInfo> _types = new();
    private readonly Dictionary<ClrMethodFullName, MethodInfo> _methods = new();

    public void AddModule(ModuleDefinition module)
    {
        foreach (var type in module.Types)
        {
            if (type.FullName == "<Module>")
            {
                continue;
            }
            
            AddType(type);
            foreach (var method in type.Methods)
            {
                AddMethod(method);
            }
        }

        foreach (var type in module.GetTypeReferences())
        {
            AddType(type);
        }
    }

    public IReadOnlyList<ClrTypeName> DefinedTypes => _types
        .Where(x => x.Value.HasBeenDefined)
        .Select(x => x.Key)
        .OrderBy(x => x.Name)
        .ToArray();

    public TypeInfo? FindType(ClrTypeName name)
    {
        return _types.GetValueOrDefault(name);
    }

    public MethodInfo? FindMethod(ClrMethodFullName name)
    {
        return _methods.GetValueOrDefault(name);
    }
    
    private void AddType(TypeDefinition definition)
    {
        var name = new ClrTypeName(definition.FullName);
        if (_types.TryGetValue(name, out var info))
        {
            if (!info.HasBeenDefined)
            {
                info.Update(definition);
            }

            return;
        }

        _types[name] = new TypeInfo(definition);
    }

    private void AddType(TypeReference reference)
    {
        var name = new ClrTypeName(reference.FullName);
        if (!_types.ContainsKey(name))
        {
            _types[name] = new TypeInfo(reference);
        }
    }

    private void AddMethod(MethodDefinition definition)
    {
        var name = new ClrMethodFullName(definition.FullName);
        if (!_methods.ContainsKey(name))
        {
            _methods[name] = new MethodInfo(definition);
        }
    }
}