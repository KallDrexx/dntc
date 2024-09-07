using Dntc.Common.Definitions;

namespace Dntc.Common.Conversion;

public class DependentMethodNode : DependencyGraphNode
{
    public ClrMethodId MethodId { get; }

    internal DependentMethodNode(DefinedMethod method)
    {
        MethodId = method.Id;
    }
}