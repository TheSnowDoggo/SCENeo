namespace SCENeo.Node;

public sealed class NodeTree
{
    public NodeTree()
    {
        Root = new Node(this);
    }

    public readonly Node Root;
}