namespace SCENeo.Ui;

public abstract class UiModifier<T> : IRenderable
    where T : IRenderable
{
    public T Source { get; set; } = default!;

    public virtual int Width { get { return Source.Width; } }
    public virtual int Height { get { return Source.Height; } }

    public bool Visible { get { return Source != null && Source.Visible; } }
    public Vec2I Offset { get { return Source.Offset; } }
    public int Layer { get { return Source.Layer; } }
    public Anchor Anchor { get { return Source.Anchor; } }

    public abstract IView<Pixel> Render();
}