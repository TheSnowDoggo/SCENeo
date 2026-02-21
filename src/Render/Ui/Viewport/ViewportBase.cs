namespace SCENeo.Ui;

public abstract class ViewportBase : UiBase, IRenderable
{
    protected sealed class Comparer : IComparer<IRenderable>
    {
        public bool Reverse { get; set; }

        public int Compare(IRenderable a, IRenderable b)
        {
            if (a == null || b == null)
            {
                return 0;
            }

            return Reverse ? b.Layer.CompareTo(a.Layer) : a.Layer.CompareTo(b.Layer);
        }
    }

    protected readonly Comparer _comparer = new();

    /// <summary>
    /// Gets or sets the width.
    /// </summary>
    public int Width { get; set; }

    /// <summary>
    /// Gets or sets the height.
    /// </summary>
    public int Height { get; set; }

    /// <summary>
    /// Gets or sets the base pixel.
    /// </summary>
    public Pixel BasePixel { get; set; }

    /// <summary>
    /// Gets or sets the source of renderables.
    /// </summary>
    public IEnumerable<IRenderable> Source { get; set; }

    public abstract IView<Pixel> Render();

    protected List<IRenderable> GetSorted()
    {
        var sorted = new List<IRenderable>();

        foreach (IRenderable renderable in Source)
        {
            if (renderable.Visible)
            {
                sorted.Add(renderable);
            }
        }

        sorted.Sort(_comparer);

        return sorted;
    }

    protected void MappedArea(IRenderable renderable, out Vec2I position, out Rect2DI area)
    {
        Vec2I size = renderable.Size();

        position = renderable.Offset + renderable.Anchor.AnchorDimension(this.Size() - size);

        area = Rect2DI.Area(position, size);
    }
}