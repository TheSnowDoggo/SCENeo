namespace SCENeo;

public sealed class DisplayMap : Image, IRenderable
{
    public DisplayMap(Pixel[,] data)
        : base(data)
    {
    }

    public DisplayMap(int width, int height)
        : base(width, height)
    {
    }

    public DisplayMap(Vec2I dimensions)
       : base(dimensions)
    {
    }

    public bool Enabled { get; set; } = true;

    public Vec2I Offset { get; set; } = Vec2I.Zero;

    public int ZOffset { get; set; } = 0;

    public Anchor Anchor { get; set; } = Anchor.None;

    public Grid2DView<Pixel> Render()
    {
        return this;
    }
}