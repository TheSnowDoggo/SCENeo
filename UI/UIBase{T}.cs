namespace SCENeo.UI;

public abstract class UIBase<T>(T source) : IRenderable
    where T : IDimensioned
{
    protected readonly T _source = source;

    public virtual int Width { get { return _source.Width; } }
    public virtual int Height { get { return _source.Height; } }

    public bool Enabled { get; set; } = true;
    public Vec2I Offset { get; set; } = Vec2I.Zero;
    public int ZOffset { get; set; } = 0;
    public Anchor Anchor { get; set; } = Anchor.None;
         
    public abstract IView<Pixel> Render();
}