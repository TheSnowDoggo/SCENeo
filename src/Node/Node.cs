namespace SCENeo.Node;

public class Node
{
    private readonly Dictionary<string, Node> _children = [];

    private NodeTree _tree;

    private Node _parent;

    private string _name = string.Empty;

    public Node(NodeTree tree = null)
    {
        if (tree != null)
        {
            _tree = tree;
        }
    }

    public string Name
    {
        get => _name;
        set
        {
            if (_name == value) return;

            if (_parent == null)
            {
                _name = value;
                return;
            }

            if (!_parent._children.Remove(_name))
            {
                throw new NodeException("Failed to find child node in parent.");
            }

            _name = value;

            AddChild(this);
        }
    }

    public Node Parent => _parent;

    public NodeTree Tree => _tree;

    public bool Active { get; set; } = true;

    public Dictionary<string, Node>.ValueCollection Children => _children.Values;

    public HashSet<string> Tags { get; set; } = [];

    public void AddChild(Node child)
    {
        if (!_children.TryAdd(child.Name, child))
        {
            throw new NodeException($"Child with name {child.Name} already exists.");
        }

        if (child == this)
        {
            throw new NodeException("Cannot add itself as a child node.");
        }

        child._parent = this;
        child._tree = _tree;

        if (_tree == null)
        {
            return;
        }

        _tree.OnAddNode(child);
    }

    public void AddChildren(IEnumerable<Node> children)
    {
        foreach (Node child in children)
        {
            AddChild(child);
        }
    }

    public void RemoveChild(string name)
    {
        if (!_children.TryGetValue(name, out Node child))
        {
            throw new NodeException($"Unknown child \'{name}\'.");
        }

        _children.Remove(name);

        child._parent = null;
        child._tree = null;

        if (_tree == null)
        {
            return;
        }

        _tree.OnRemoveNode(child);
    }

    public Node GetChild(string name)
    {
        return _children.GetValueOrDefault(name);
    }

    public T GetChild<T>(string name)
        where T : Node
    {
        return (T)GetChild(name);
    }

    public Node GetNode(string path)
    {
        Node current = this;

        foreach (string node in path.Trim('/').Split('/'))
        {
            current = current.GetChild(node);

            if (current == null)
            {
                return null;
            }
        }

        return current;
    }

    public T GetNode<T>(string path) 
        where T : Node
    {
        return (T)GetNode(path);
    }

    /// <summary>
    /// Called on start.
    /// </summary>
    public virtual void Start()
    {
    }

    /// <summary>
    /// Called on update.
    /// </summary>
    /// <param name="delta">The time elapsed since last frame in seconds.</param>
    public virtual void Update(double delta)
    {
    }

    public IEnumerable<Node> ActiveDescendents()
    {
        if (!Active)
        {
            yield break;
        }

        var stack = new Stack<Node>();

        stack.Push(this);

        while (stack.TryPop(out Node node))
        {
            yield return node;

            foreach (Node child in node.Children)
            {
                if (child.Active)
                {
                    stack.Push(child);
                }
            }
        }
    }

    public IEnumerable<Node> Descendents()
    {
        var stack = new Stack<Node>();

        stack.Push(this);

        while (stack.TryPop(out Node node))
        {
            yield return node;

            foreach (Node child in node.Children)
            {
                stack.Push(child);
            }
        }
    }
}