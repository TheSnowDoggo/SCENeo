namespace SCENeo;

public interface IRenderable
{
    bool Active { get; }

    public Vec2I Position { get; }

    public int ZOffset { get; }

    public Anchor Anchor { get; }

    Grid2DView<Pixel> Render();
}