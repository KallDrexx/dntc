using Mono.Cecil;

namespace Dntc.Common;

public class TypeInfo
{
    internal TypeInfo(TypeDefinition definition)
    {
        Update(definition);
    }

    internal TypeInfo(TypeReference reference)
    {
        HasBeenDefined = false;
        ClrName = new ClrTypeName(reference.FullName);
        IsValueType = null;
    }
    
    public ClrTypeName ClrName { get; private set; }
    public bool? IsValueType { get; private set; }
    public IReadOnlyList<ClrMethodFullName> Methods { get; private set; } = Array.Empty<ClrMethodFullName>();
    public bool HasBeenDefined { get; private set; }

    internal void Update(TypeDefinition definition)
    {
        HasBeenDefined = true;
        IsValueType = definition.IsValueType;
        ClrName = new ClrTypeName(definition.FullName);
        Methods = definition.Methods
            .Select(x => new ClrMethodFullName(x.FullName))
            .ToArray();
    }
}