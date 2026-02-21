namespace SCENeo.Ui;

public sealed class DisplayMap : Image, IRenderable
{
    public DisplayMap() { }
    public DisplayMap(Pixel[,] data) : base(data) { }
    public DisplayMap(Grid2D<Pixel> grid) : base(grid) { }
    public DisplayMap(int width, int height) : base(width, height) { }
    public DisplayMap(Vec2I size) : base(size) { }

    /// <inheritdoc cref="UiBase.Visible"/>
    public bool Visible { get; set; } = true;

    /// <inheritdoc cref="UiBase.Offset"/>
    public Vec2I Offset { get; set; }

    /// <inheritdoc cref="UiBase.Layer"/>
    public int Layer { get; set; }

    /// <inheritdoc cref="UiBase.Anchor"/>
    public Anchor Anchor { get; set; }

    /// <inheritdoc cref="Image.Plain(int, int, Pixel)"/>
    public static new DisplayMap Plain(int width, int height, Pixel pixel)
    {
        var image = new DisplayMap(width, height);

        image.Fill(pixel);

        return image;
    }

    /// <inheritdoc cref="Image.Plain(Vec2I, Pixel)"/>
    public new static DisplayMap Plain(Vec2I size, Pixel pixel)
    {
        return Plain(size.X, size.Y, pixel);
    }

    public IView<Pixel> Render()
    {
        return AsReadonly();
    }
}