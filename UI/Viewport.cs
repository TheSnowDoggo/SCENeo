namespace SCENeo.Ui;

public sealed partial class Viewport : IRenderable
{
    private readonly Image _buffer = new Image();

    public Viewport()
    {
    }

    public bool Enabled { get; set; } = true;
    public Vec2I Offset { get; set; }
    public int ZIndex { get; set; }
    public Anchor Anchor { get; set; }

    public int Width { get; set; }
    public int Height { get; set; }

    public Pixel BasePixel { get; set; } = Pixel.Black;

    public List<IRenderable> Renderables { get; init; } = [];

    public IView<Pixel> Render()
    {
        Update();

        return _buffer.AsView();
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

            Vec2I anchorOffset = renderable.Anchor.AnchorDimension(renderSize - size);

            Vec2I position = renderable.Offset + anchorOffset;

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
        var sorted = new List<IRenderable>(Renderables.Count);

        foreach (IRenderable renderable in Renderables)
        {
            if (renderable.Enabled)
            {
                sorted.Add(renderable);
            }
        }

        sorted.Sort(Comparer.Instance);

        return sorted;
    }
}