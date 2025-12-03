using SCENeo.Utils;

namespace SCENeo.UI;

public sealed class Viewport : UIBaseImage, IResizeable
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

    public List<IRenderable> Renderables { get; init; } = [];

    public Pixel BasePixel { get; set; } = new(SCEColor.Black);

    public void Resize(int width, int height)
    {
        _source.Resize(width, height);
    }

    protected override void Update()
    {
        _source.Fill(BasePixel);

        var sorted = new List<IRenderable>(Renderables.Count);

        foreach (IRenderable renderable in Renderables)
        {
            if (!renderable.Enabled) continue;

            sorted.Add(renderable);
        }

        sorted.Sort(Comparer.Instance);

        foreach (IRenderable renderable in sorted)
        {
            IView<Pixel> view = renderable.Render();

            Vec2I anchorOffset = renderable.Anchor.AnchorDimension(this.Dimensions() - view.Dimensions());

            _source.MergeMap(view, anchorOffset + renderable.Offset);
        }
    }
}