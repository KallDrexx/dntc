using Mono.Cecil;

namespace Dntc.Common.Definitions.Mutators;

/// <summary>
/// Allows changing method definition properties
/// </summary>
public interface IMethodDefinitionMutator
{
    void Mutate(DefinedMethod method, MethodDefinition cecilDefinition);
}