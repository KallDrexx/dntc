using Dntc.Common.Definitions.ReferenceTypeSupport;
using Mono.Cecil;

namespace Dntc.Common.Definitions.Definers;

/// <summary>
/// Basic mechanism for creating a `DotNetDefinedMethod`
/// </summary>
public class DefaultDotNetMethodDefiner : IDotNetMethodDefiner
{
    private readonly IMemoryManagementActions _memoryManagement;

    public DefaultDotNetMethodDefiner(IMemoryManagementActions memoryManagement)
    {
        _memoryManagement = memoryManagement;
    }

    public DefinedMethod Define(MethodDefinition method)
    {
        return new DotNetDefinedMethod(method, _memoryManagement);
    }
}