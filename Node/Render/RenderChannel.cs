using SCENeo.Ui;

namespace SCENeo.Node.Render;

public sealed class RenderChannel : IRenderable
{
    private readonly Image _buffer = new Image();

    public RenderChannel()
    {
    }

    public bool Enabled { get; set; } = true;
    public Vec2I Offset { get; set; }
    public int ZIndex { get; set; }
    public Anchor Anchor { get; set; }

    public int Width { get; set; }
    public int Height { get; set; }

    public Pixel BasePixel { get; set; }

    public void Initialize()
    {
        if (Width != _buffer.Width || Height != _buffer.Height)
        {
            _buffer.CleanResize(Width, Height);
        }

        _buffer.Fill(BasePixel);
    }

    public void Load(IView<Pixel> view, Vec2I position)
    {
        _buffer.MergeMap(view, position);
    }

    public IView<Pixel> Render()
    {
        return _buffer.AsView();
    }
}