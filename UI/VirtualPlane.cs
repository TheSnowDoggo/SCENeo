namespace SCENeo.UI;

/// <summary>
/// Represents a virtual grid of identical pixels.
/// </summary>
/// <remarks>
/// Good for creating large planes with low memory usage.
/// </remarks>
internal class VirtualPlane : IRenderable
{
    private readonly PlainView2D<Pixel> _pv = [];

    public VirtualPlane() { }
    public VirtualPlane(int width, int height)
    {
        _pv = new PlainView2D<Pixel>()
        {
            Width  = width,
            Height = height,
        };
    } 
    public VirtualPlane(Vec2I dimensions) : this(dimensions.X, dimensions.Y) { }

    public Pixel Value
    {
        get { return _pv.Value; }
        set { _pv.Value = value; }
    }

    public int Width
    {
        get { return _pv.Width; }
        set { _pv.Width = value; }
    }

    public int Height
    {
        get { return _pv.Height; }
        set { _pv.Height = value; }
    }

    public bool Enabled { get; set; } = true;
    public Vec2I Offset { get; set; }
    public int ZOffset { get; set; }
    public Anchor Anchor { get; set; }

    public IView<Pixel> Render()
    {
        return _pv;
    }
}
