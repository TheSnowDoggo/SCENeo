namespace SCENeo.Node;

public sealed class Sprite2D<T> : Node2D, IRenderable
    where T : IRenderable
{
    public T? Source { get; set; } = default;

    public bool Enabled { get { return Source != null; } }

    public Vec2I Offset { get { return (Vec2I)GlobalPosition.Round() + GetSource().Offset; } }

    public int ZOffset { get { return GetSource().ZOffset; } }

    public Anchor Anchor { get { return GetSource().Anchor; } }

    public Grid2DView<Pixel> Render()
    {
        return GetSource().Render();
    }

    private IRenderable GetSource()
    {
        return Source ?? throw new NullReferenceException("Source was null.");
    }
}