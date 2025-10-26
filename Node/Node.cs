using System.Text;

namespace SCENeo.Node;

public class Node
{
    private readonly Dictionary<string, Node> _children = [];

    private NodeTree? _tree = null;

    private Node? _parent = null;

    private string _name = string.Empty;

    private bool _active = true;

    private bool _globalActive = true;

    public Node(NodeTree? tree = null)
    {
        if (tree != null) _tree = tree;
    }

    public string Name
    {
        get { return _name; }
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

    public Node? Parent { get { return _parent; } }

    public bool Active
    {
        get { return _active; }
        set
        {
            _active = value;

            if (_globalActive != _active)
            {
                UpdateChildrenGlobalActive();
            }
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

        if (child == this)
        {
            throw new NodeException("Cannot add itself as a child node.");
        }

        child._parent = this;
        child._tree   = _tree;

        if (child._globalActive != _globalActive)
        {
            child.UpdateChildrenGlobalActive();
        }

        _tree?.OnNodeAdded?.Invoke(child);
    }

    public void RemoveChild(string name)
    {
        if (!_children.TryGetValue(name, out Node? child))
        {
            throw new NodeException($"Unknown child \'{name}\'.");
        }

        _children.Remove(name);

        child._parent = null;
        child._tree   = null;

        if (child._globalActive != child._active)
        {
            child.UpdateChildrenGlobalActive();
        }

        _tree?.OnNodeRemoved?.Invoke(child);
    }

    public Node GetChild(string name)
    {
        return _children[name];
    }

    public T GetChild<T>(string name) where T : Node
    {
        return (T)GetChild(name);
    }

    public Node GetNode(string path)
    {
        if (path == string.Empty)
        {
            return GetChild(path);
        }

        Node current = this;

        foreach (string node in path.Trim('/').Split('/'))
        {
            current = current.GetChild(node);
        }

        return current;
    }

    public T GetNode<T>(string path) where T : Node
    {
        return (T)GetNode(path);
    }

    public virtual void Start()
    {
    }

    public virtual void Update(double delta)
    {
    }

    public IEnumerable<Node> ActiveDescendents()
    {
        if (!Active) yield break;

        var stack = new Stack<Node>();

        stack.Push(this);

        while (stack.TryPop(out Node? node))
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

    private void UpdateChildrenGlobalActive()
    {
        _globalActive = Parent == null ? _active : Parent._globalActive && _active;

        foreach (Node child in Children)
        {
            child.UpdateChildrenGlobalActive();
        }
    }
}