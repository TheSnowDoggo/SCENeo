namespace SCENeo;

public interface IRenderable
{
    bool Enabled { get; }

    public Vec2I Offset { get; }

    public int ZOffset { get; }

    public Anchor Anchor { get; }

    Grid2DView<Pixel> Render();
}