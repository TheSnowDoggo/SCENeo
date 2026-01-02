namespace SCENeo;

public interface IRenderable : IDimensioned
{
    bool Visible { get; }
    public Vec2I Offset { get; }
    public int ZIndex { get; }
    public Anchor Anchor { get; }
    IView<Pixel> Render();
}