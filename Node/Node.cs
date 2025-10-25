namespace SCENeo.Node;

public class Node
{
    private readonly Dictionary<string, Node> _children = [];

    private Node? _parent = null;

    private string _name = string.Empty;

    private bool _active = true;

    private bool _globalActive = true;

    public string Name
    {
        get { return _name; }
        set
        {
            if (_name == value)
            {
                return;
            }

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

    public Node? Parent { get { return _parent; } }

    public bool Active
    {
        get { return _active; }
        set
        {
            if (_active == value)
            {
                return;
            }

            _active = value;

            UpdateChildrenGlobalActive();
        }
    }

    public bool GlobalActive { get { return _globalActive; } }

    public Dictionary<string, Node>.ValueCollection Children { get { return _children.Values; } }

    public void AddChild(Node child)
    {
        if (!_children.TryAdd(child.Name, child))
        {
            throw new NodeException($"Child with name {child.Name} already exists.");
        }

        child._parent = this;
    }

    public void RemoveChild(string name)
    {
        if (!_children.TryGetValue(name, out Node? child))
        {
            throw new NodeException($"Unknown child \'{name}\'.");
        }

        _children.Remove(name);
        child._parent = null;
    }

    public Node GetChild(string name)
    {
        return _children[name];
    }

    public T GetChild<T>(string name) 
        where T : Node
    {
        return (T)GetChild(name);
    }

    public List<Node> ActiveDescendents()
    {
        if (!GlobalActive)
        {
            return [];
        }

        var nodes = new List<Node>();
        var stack = new Stack<Node>();

        stack.Push(this);

        while (stack.TryPop(out Node? node))
        {
            nodes.Add(node);

            foreach (Node child in node.Children)
            {
                if (child.Active)
                {
                    stack.Push(child);
                }
            }
        }

        return nodes;
    }

    private void UpdateChildrenGlobalActive()
    {
        _globalActive = Parent == null ? _active : Parent._globalActive && _active;

        foreach (Node child in Children)
        {
            child.UpdateChildrenGlobalActive();
        }
    }
}