using SCENeo.Utils;

namespace SCENeo.UI;

public sealed class Viewport : UIBaseImage
{
    private sealed class Comparer : IComparer<IRenderable?>
    {
        private static readonly Lazy<Comparer> lazy = new(() => new Comparer());

        private Comparer() { }

        public static Comparer Instance { get { return lazy.Value; } }

        public int Compare(IRenderable? x, IRenderable? y)
        {
            if (x == null || y == null)
            {
                throw new NullReferenceException("Renderable was null.");
            }
            return x.ZOffset.CompareTo(y.ZOffset);
        }
    }

    public Viewport() : base() { }

    public Viewport(int width, int height) : base(width, height) { }

    public Viewport(Vec2I dimensions) : base(dimensions) { }

    public readonly List<IRenderable> Renderables = [];

    public Pixel BasePixel = new(SCEColor.Black);

    protected override void Update()
    {
        _data.Fill(BasePixel);

        var sorted = new List<IRenderable>(Renderables.Count);

        foreach (IRenderable renderable in Renderables)
        {
            if (!renderable.Enabled)
            {
                continue;
            }

            sorted.Add(renderable);
        }

        sorted.Sort(Comparer.Instance);

        foreach (IRenderable renderable in sorted)
        {
            Grid2DView<Pixel> view = renderable.Render();

            Vec2I anchorOffset = renderable.Anchor.AnchorDimension(Dimensions - view.Dimensions);

            _data.MergeMap(view, anchorOffset + renderable.Offset);
        }
    }
}