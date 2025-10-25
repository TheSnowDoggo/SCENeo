using SCENeo.Utils;

namespace SCENeo.UI;

internal sealed class Viewport : UIBase
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

    public Viewport(int width, int height) : base(width, height) { }

    public Viewport(Vec2I dimensions) : base(dimensions) { }

    public readonly List<IRenderable> Renderables = [];

    public Pixel BaseColor = new(SCEColor.Black);

    private void Update()
    {
        _image.Fill(BaseColor);

        var sorted = new List<IRenderable>(Renderables.Count);

        foreach (IRenderable renderable in Renderables)
        {
            if (!renderable.Active)
            {
                continue;
            }

            sorted.Add(renderable);
        }

        sorted.Sort(Comparer.Instance);

        foreach (IRenderable renderable in sorted)
        {
            Grid2DView<Pixel> view = renderable.Render();

            Vec2I anchorOffset = renderable.Anchor.AnchorDimension(view.Dimensions, Dimensions);

            _image.MergeMap(view, anchorOffset + renderable.Position);
        }
    }

    public override Grid2DView<Pixel> Render()
    {
        Update();

        return _image;
    }
}