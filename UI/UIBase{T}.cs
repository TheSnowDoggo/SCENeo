namespace SCENeo.UI;

public abstract class UIBase<T>(T data) : IRenderable, IDimensioned
    where T : IDimensioned
{
    protected readonly T _data = data;

    public int Width { get { return _data.Width; } }

    public int Height { get { return _data.Height; } }

    public int Size { get { return _data.Size; } }

    public Vec2I Dimensions { get { return _data.Dimensions; } }

    public bool Active { get; set; } = true;

    public Vec2I Position { get; set; } = Vec2I.Zero;

    public int ZOffset { get; set; } = 0;

    public Anchor Anchor { get; set; } = Anchor.None;

    public abstract Grid2DView<Pixel> Render();
}