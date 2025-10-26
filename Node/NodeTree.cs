namespace SCENeo.Node;

public sealed class NodeTree
{
    public NodeTree()
    {
        Root = new Node(this);
    }

    public readonly Node Root;

    public Action<Node>? OnNodeAdded { get; init; }

    public Action<Node>? OnNodeRemoved { get; init; }
}