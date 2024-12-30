using Mono.Cecil;

namespace Dntc.Common.Definitions.Definers;

/// <summary>
/// Creates a DNTC `DefinedType` for the specified Mono.cecil `TypeDefinition`
/// </summary>
public interface IDotNetTypeDefiner
{
    DefinedType Define(TypeDefinition type);
}