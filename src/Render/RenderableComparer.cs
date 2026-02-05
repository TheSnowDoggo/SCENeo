namespace SCENeo.Ui;

public sealed class RenderableComparer : IComparer<IRenderable>
{
    private static readonly Lazy<RenderableComparer> Lazy = new(() => new());

    private RenderableComparer() { }

    public static RenderableComparer Instance => Lazy.Value;

    public int Compare(IRenderable x, IRenderable y)
    {
        return x != null && y != null ? x.Layer.CompareTo(y.Layer) : 0;
    }
}
