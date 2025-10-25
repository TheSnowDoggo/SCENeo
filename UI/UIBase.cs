namespace SCENeo.UI;

public abstract class UIBase : IRenderable
{
    protected Image _image;

    public UIBase(int width, int height)
    {
        _image = new Image(width, height);
    }

    public UIBase(Vec2I dimensions)
    {
        _image = new Image(dimensions);
    }

    public int Width { get { return _image.Width; } }

    public int Height { get { return _image.Height; } }

    public int Size { get { return _image.Size; } }

    public Vec2I Dimensions { get { return _image.Dimensions; } }

    public bool Active { get; set; } = true;

    public Vec2I Position { get; set; } = Vec2I.Zero;

    public int ZOffset { get; set; } = 0;

    public Anchor Anchor { get; set; } = Anchor.None;

    public abstract Grid2DView<Pixel> Render();
}