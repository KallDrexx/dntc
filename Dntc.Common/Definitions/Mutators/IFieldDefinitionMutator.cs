using Mono.Cecil;

namespace Dntc.Common.Definitions.Mutators;

/// <summary>
/// Allows changing field definition.
/// </summary>
public interface IFieldDefinitionMutator
{
    void Mutate(DefinedField field, FieldDefinition cecilField);
}