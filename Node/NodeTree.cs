namespace SCENeo.Node;

public sealed class NodeTree
{
    public NodeTree()
    {
        Root = new Node(this);
    }

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
}