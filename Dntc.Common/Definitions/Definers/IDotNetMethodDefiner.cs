using Mono.Cecil;

namespace Dntc.Common.Definitions.Definers;

/// <summary>
/// Generates a DNTC defined method based on a Mono.Cecil method.
/// </summary>
public interface IDotNetMethodDefiner
{
    DefinedMethod Define(MethodDefinition method);
}