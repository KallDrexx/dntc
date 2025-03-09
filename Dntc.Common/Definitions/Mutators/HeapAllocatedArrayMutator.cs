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

        if (_definitionCatalog.Get(field.IlName) == null)
        {
            var typeDefinition = new HeapArrayDefinedType(cecilField.FieldType);
            _definitionCatalog.Add([typeDefinition]);
        }
    }
}