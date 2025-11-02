namespace SCENeo.Node;

public sealed class NodeManager
{
    public readonly NodeTree Tree;

    public NodeManager()
    {
        Tree = new NodeTree();
    }

    public List<IEngine> Engines { get; init; } = [];

    public void Start()
    {
        foreach (Node descendant in Tree.Root.ActiveDescendents())
        {
            descendant.Start();
        }
    }

    public void Update(double delta)
    {
        var active = new List<Node>();

        foreach (Node node in Tree.Root.ActiveDescendents())
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

        Tree.ClearRemoveQueue();
    }
}
