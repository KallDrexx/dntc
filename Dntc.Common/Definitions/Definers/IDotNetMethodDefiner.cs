using Mono.Cecil;

namespace Dntc.Common.Definitions.Definers;

public interface IDotNetMethodDefiner
{
    /// <summary>
    /// Attempts to generate a DNTC defined method based on a Mono.Cecil method. Returns
    /// `null` if this definer is not able to create a definition for the provided method.
    /// </summary>
    DefinedMethod? Define(MethodDefinition method);
}