namespace SCENeo.Ui;

internal sealed class CanvasItem
{
    private readonly Dictionary<string, CanvasItem> _children = [];

    public CanvasItem()
    {
    }

    public IRenderable Renderable { get; set; } = null!;

    public bool Enabled { get; set; } = true;
    public Vec2I Offset { get; set; }
    public int ZOffset { get; set; }

    public Dictionary<string, CanvasItem>.ValueCollection Children { get { return _children.Values; } }
}
