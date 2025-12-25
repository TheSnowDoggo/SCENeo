namespace SCENeo.Ui;

public abstract class UiModifier<T>(T source) : IRenderable
    where T : IRenderable
{
    protected readonly T _source = source;

    public virtual int Width { get { return _source.Width; } }
    public virtual int Height { get { return _source.Height; } }

    public bool Enabled { get { return _source.Enabled; } }
    public Vec2I Offset { get { return _source.Offset; } }
    public int ZIndex { get { return _source.ZIndex; } }
    public Anchor Anchor { get { return _source.Anchor; } }

    public abstract IView<Pixel> Render();
}