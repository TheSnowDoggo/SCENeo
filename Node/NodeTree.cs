namespace SCENeo.Node;

public sealed class NodeTree
{
    private readonly Queue<Node> _removeQueue = [];

    public NodeTree()
    {
        Root = new Node(this);
    }

    public readonly Node Root;

    public void QueueRemove(Node node)
    {
        _removeQueue.Enqueue(node);
    }

    public void ClearRemoveQueue()
    {
        while (_removeQueue.TryDequeue(out Node? node))
        {
            if (node.Parent == null) continue;

            node.Parent.RemoveChild(node.Name);
        }
    }
}