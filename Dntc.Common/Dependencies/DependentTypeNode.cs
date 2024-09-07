using Dntc.Common.Definitions;

namespace Dntc.Common.Conversion;

public class DependentTypeNode : DependencyGraphNode
{
    public ClrTypeName TypeName { get; }

    internal DependentTypeNode(DefinedType type)
    {
        TypeName = type.ClrName;
    }
}