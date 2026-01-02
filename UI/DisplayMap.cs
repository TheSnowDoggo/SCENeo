namespace SCENeo.Ui;

public sealed class DisplayMap : Image, IRenderable
{
    public DisplayMap(Pixel[,] data)  : base(data) { }
    public DisplayMap(Grid2D<Pixel> grid) : base(grid) { }
    public DisplayMap() : base() { }
    public DisplayMap(int width, int height) : base(width, height) { }
    public DisplayMap(Vec2I dimensions) : base(dimensions) { }

    public bool Visible { get; set; } = true;

    public Vec2I Offset { get; set; } = Vec2I.Zero;

    public int ZIndex { get; set; } = 0;

    public Anchor Anchor { get; set; } = Anchor.None;

    public IView<Pixel> Render()
    {
        return (Grid2DView<Pixel>)this;
    }
}