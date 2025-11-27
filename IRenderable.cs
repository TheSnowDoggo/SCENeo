namespace SCENeo;

public interface IRenderable : IDimensioned
{
    bool Enabled { get; }

    public Vec2I Offset { get; }

    public int ZOffset { get; }

    public Anchor Anchor { get; }

    IView<Pixel> Render();
}