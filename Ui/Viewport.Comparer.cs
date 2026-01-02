namespace SCENeo.Ui;

public sealed partial class Viewport
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

            return x.Layer.CompareTo(y.Layer);
        }
    }
}
