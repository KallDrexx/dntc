namespace Dntc.Common.Conversion;

public abstract class DependencyGraphNode
{
    private readonly List<DependencyGraphNode> _dependencies = new();

    public IReadOnlyList<DependencyGraphNode> Dependencies => _dependencies;

    internal void AddChild(DependencyGraphNode node)
    {
        _dependencies.Add(node);
    }
}