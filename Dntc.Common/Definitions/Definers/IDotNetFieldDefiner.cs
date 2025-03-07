using Mono.Cecil;

namespace Dntc.Common.Definitions.Definers;

public interface IDotNetFieldDefiner
{
    /// <summary>
    /// Attempts to create a dntc `Definedglobal` for the specified Mono.cecil field. Returns `null`
    /// if this definer is not able to create a definition for this field.
    /// </summary>
    /// <param name="field">The mono.cecil definition for the field</param>
    /// <param name="fieldType">A dntc type definition for the field's type</param>
    DefinedField? Define(FieldDefinition field, DefinedType fieldType);
}