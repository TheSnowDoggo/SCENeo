namespace SCENeo.UI;

public abstract class UIModifier<T>(T source) : IRenderable, IDimensioned
    where T : IRenderable, IDimensioned
{
    protected readonly T _source = source;

    public int Width { get { return _source.Width; } }
    public int Height { get { return _source.Height; } }
    public int Size { get { return _source.Size; } }
    public Vec2I Dimensions { get { return _source.Dimensions; } }

    public bool Enabled { get { return _source.Enabled; } }
    public Vec2I Offset { get { return _source.Offset; } }
    public int ZOffset { get { return _source.ZOffset; } }
    public Anchor Anchor { get { return _source.Anchor; } }

    public abstract Grid2DView<Pixel> Render();
}