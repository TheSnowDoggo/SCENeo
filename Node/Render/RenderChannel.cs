using SCENeo.Ui;

namespace SCENeo.Node.Render;

public sealed partial class RenderChannel : UiBase, IRenderable
{
    private sealed class RenderItem : IComparable<RenderItem>
    {
        private readonly int _layer;

        public readonly IView<Pixel> View;
        public readonly Vec2I Position;

        public RenderItem(IView<Pixel> view, Vec2I position, int layer)
        {
            View = view;
            Position = position;
            _layer = layer;
        }

        public int CompareTo(RenderItem? other)
        {
            return other != null ? _layer.CompareTo(other._layer) : 0;
        }
    }

    private readonly Image _buffer = new Image();

    private readonly List<RenderItem> _items = [];

    public RenderChannel()
    {
    }

    /// <summary>
    /// Gets or sets the width.
    /// </summary>
    public int Width { get; set; }

    /// <summary>
    /// Gets or sets the height.
    /// </summary>
    public int Height { get; set; }

    /// <summary>
    /// Gets or sets the base pixel.
    /// </summary>
    public Pixel BasePixel { get; set; }

    public void Initialize()
    {
        if (Width != _buffer.Width || Height != _buffer.Height)
        {
            _buffer.CleanResize(Width, Height);
        }

        _buffer.Fill(BasePixel);
    }

    public void Load(IView<Pixel> view, Vec2I position, int layer)
    {
        _items.Add(new RenderItem(view, position, layer));
    }

    public IView<Pixel> Render()
    {
        _items.Sort();

        foreach (RenderItem item in _items)
        {
            _buffer.MergeMap(item.View, item.Position);
        }

        _items.Clear();

        return _buffer.AsReadonly();
    }
}