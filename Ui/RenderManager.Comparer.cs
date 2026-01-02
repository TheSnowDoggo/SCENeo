namespace SCENeo.Ui;

public sealed partial class RenderManager
{
    private sealed class Comparer : IComparer<IRenderSource>
    {
        private static readonly Lazy<Comparer> Lazy = new(() => new Comparer());

        private Comparer() { }

        public static Comparer Instance { get { return Lazy.Value; } }

        public int Compare(IRenderSource? a, IRenderSource? b)
        {
            if (a == null || b == null) return 0;

            return a.Priority.CompareTo(b.Priority);
        }
    }
}
