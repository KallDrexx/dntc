using Mono.Cecil;

namespace Dntc.Common.Definitions.Definers;

/// <summary>
/// Basic mechanism for creating a `DotNetDefinedType` for a Mono.cecil type.
/// </summary>
public class DefaultTypeDefiner : IDotNetTypeDefiner
{
    public DefinedType Define(TypeDefinition type)
    {
        return new DotNetDefinedType(type);
    }
}