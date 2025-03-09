using Dntc.Common.Definitions.CustomDefinedTypes;
using Mono.Cecil;

namespace Dntc.Common.Definitions.Mutators;

/// <summary>
/// Sets up the relevant types in a definition for any unhandled array types use the transpiled
/// heap allocated array defined type.
/// </summary>
public class HeapAllocatedArrayMutator : IFieldDefinitionMutator
{
    private readonly DefinitionCatalog _definitionCatalog;

    public HeapAllocatedArrayMutator(DefinitionCatalog definitionCatalog)
    {
        _definitionCatalog = definitionCatalog;
    }

    public void Mutate(DefinedField field, FieldDefinition cecilField)
    {
        if (!cecilField.FieldType.IsArray)
        {
            return;
        }

        // This is a hack, but we need to ensure we don't create a heap allocated array
        // when it's already been mutated as part of being a statically sized array. This works
        // because the statically sized array mutator adjusts the ILName. We need a better way to
        // detect if the heap allocated array definition should take ownership of this array or not.
        if (!field.IlName.Value.EndsWith("[]"))
        {
            return;
        }

        if (_definitionCatalog.Get(field.IlName) == null)
        {
            var typeDefinition = new HeapArrayDefinedType(cecilField.FieldType);
            _definitionCatalog.Add([typeDefinition]);
        }
    }
}