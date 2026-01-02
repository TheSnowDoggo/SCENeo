namespace SCENeo.Ui;

public sealed partial class Viewport : UiBase, IRenderable
{
    private readonly Image _buffer = new Image();

    public Viewport()
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

    public Pixel BasePixel { get; set; } = Pixel.Black;

    public IEnumerable<IRenderable> Source { get; set; } = default!;

    public IView<Pixel> Render()
    {
        Update();

        return _buffer.AsReadonly();
    }

    private void Update()
    {
        if (Width != _buffer.Width || Height != _buffer.Height)
        {
            _buffer.CleanResize(Width, Height);
        }

        _buffer.Fill(BasePixel);

        Rect2DI renderArea = new Rect2DI(Width, Height);

        Vec2I renderSize = new Vec2I(Width, Height);

        foreach (IRenderable renderable in GetSorted())
        {
            Vec2I size = renderable.Size();

            Vec2I position = renderable.Offset + renderable.Anchor.AnchorDimension(renderSize - size);

            Rect2DI area = Rect2DI.Area(position, size);

            if (!renderArea.Overlaps(area))
            {
                continue;
            }

            _buffer.MergeMap(renderable.Render(), position);
        }
    }

    private List<IRenderable> GetSorted()
    {
        var sorted = new List<IRenderable>();

        foreach (IRenderable renderable in Source)
        {
            if (renderable.Visible)
            {
                sorted.Add(renderable);
            }
        }

        sorted.Sort(Comparer.Instance);

        return sorted;
    }
}