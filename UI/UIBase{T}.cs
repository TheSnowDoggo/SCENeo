namespace SCENeo.UI;

public abstract class UIBase<T>(T source) : IRenderable, IResizeable
    where T : IDimensioned, IResizeable
{
    protected readonly T _source = source;

    public int Width { get { return _source.Width; } }
    public int Height { get { return _source.Height; } }

    public bool Enabled { get; set; } = true;
    public Vec2I Offset { get; set; } = Vec2I.Zero;
    public int ZOffset { get; set; } = 0;
    public Anchor Anchor { get; set; } = Anchor.None;

    public virtual void Resize(int width, int height)
    {
        _source.Resize(width, height);
    }
         
    public abstract IView<Pixel> Render();
}