namespace SCENeo.Node;

public sealed class NodeTree
{
    public NodeTree()
    {
        Root = new Node(this)
        {
            Name = "Root",
        };
    }

    public event Action<Node> AddNode;
    public event Action<Node> RemoveNode;


    public readonly Node Root;

    public List<IEngine> Engines { get; init; } = [];

    public void Start()
    {
        foreach (Node descendant in Root.ActiveDescendents())
        {
            descendant.Start();
        }
    }

    public void Update(double delta)
    {
        var active = new List<Node>();

        foreach (Node node in Root.ActiveDescendents())
        {
            active.Add(node);

            node.Update(delta);
        }

        foreach (IEngine engine in Engines)
        {
            if (engine.Enabled)
            {
                engine.Update(delta, active);
            }
        }
    }

    internal void OnAddNode(Node node)
    {
        AddNode?.Invoke(node);
    }

    internal void OnRemoveNode(Node node)
    {
        RemoveNode?.Invoke(node);
    }
}