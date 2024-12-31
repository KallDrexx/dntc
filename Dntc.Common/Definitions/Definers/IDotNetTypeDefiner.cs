using Mono.Cecil;

namespace Dntc.Common.Definitions.Definers;

public interface IDotNetTypeDefiner
{
    /// <summary>
    /// Creates a DNTC `DefinedType` for the specified Mono.cecil `TypeDefinition` if able to.
    /// If the definer is not able to handle the passed in type, then a `null` is returned.
    /// </summary>
    DefinedType? Define(TypeDefinition type);
}