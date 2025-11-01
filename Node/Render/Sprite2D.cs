namespace SCENeo.Node.Render;

public sealed class Sprite2D : Node2D, IRenderable
{
    public IRenderable? Source { get; set; } = default;

    public int Width { get { return GetSource().Width; } }

    public int Height { get { return GetSource().Height; } }

    public bool Enabled { get { return Source != null; } }

    public Vec2I Offset { get { return (Vec2I)GlobalPosition.Round() * new Vec2I(2, 1) + GetSource().Offset; } }

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