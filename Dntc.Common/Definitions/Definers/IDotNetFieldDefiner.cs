using Mono.Cecil;

namespace Dntc.Common.Definitions.Definers;

public interface IDotNetFieldDefiner
{
    /// <summary>
    /// Attempts to create a dntc `Definedglobal` for the specified Mono.cecil field. Returns `null`
    /// if this definer is not able to create a definition for this field.
    /// </summary>
    DefinedField? Define(FieldDefinition field);
}