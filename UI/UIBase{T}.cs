namespace SCENeo.UI;

public abstract class UIBase<T>(T data) : IRenderable, IDimensioned, IResizeable
    where T : IDimensioned, IResizeable
{
    protected readonly T _data = data;

    public int Width { get { return _data.Width; } }

    public int Height { get { return _data.Height; } }

    public int Size { get { return _data.Size; } }

    public Vec2I Dimensions { get { return _data.Dimensions; } }

    public bool Enabled { get; set; } = true;

    public Vec2I Offset { get; set; } = Vec2I.Zero;

    public int ZOffset { get; set; } = 0;

    public Anchor Anchor { get; set; } = Anchor.None;

    public virtual void Resize(int width, int height)
    {
        _data.Resize(width, height);
    }
         
    public abstract Grid2DView<Pixel> Render();
}