namespace SCENeo.Ui;

public sealed class DisplayMap : Image, IRenderable
{
    public DisplayMap(Pixel[,] data)  : base(data) { }
    public DisplayMap(Grid2D<Pixel> grid) : base(grid) { }
    public DisplayMap() : base() { }
    public DisplayMap(int width, int height) : base(width, height) { }
    public DisplayMap(Vec2I dimensions) : base(dimensions) { }

    /// <inheritdoc cref="UiBase.Visible"/>
    public bool Visible { get; set; } = true;

    /// <inheritdoc cref="UiBase.Offset"/>
    public Vec2I Offset { get; set; }

    /// <inheritdoc cref="UiBase.Layer"/>
    public int Layer { get; set; }

    /// <inheritdoc cref="UiBase.Anchor"/>
    public Anchor Anchor { get; set; }

    public IView<Pixel> Render()
    {
        return AsReadonly();
    }
}