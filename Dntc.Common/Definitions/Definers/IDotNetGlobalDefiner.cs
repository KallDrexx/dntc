using Mono.Cecil;

namespace Dntc.Common.Definitions.Definers;

/// <summary>
/// Creates a dntc `Definedglobal` for the specified Mono.cecil field.
/// </summary>
public interface IDotNetGlobalDefiner
{
    DefinedGlobal Define(FieldDefinition field);
}