using Mono.Cecil;

namespace Dntc.Common.Definitions.Definers;

/// <summary>
/// Basic mechanism for creating a `DotNetDefinedMethod`
/// </summary>
public class DefaultDotNetMethodDefiner : IDotNetMethodDefiner
{
    public DefinedMethod Define(MethodDefinition method)
    {
        return new DotNetDefinedMethod(method);
    }
}