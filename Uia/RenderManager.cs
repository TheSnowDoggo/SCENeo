using System.Collections;

namespace SCENeo.Ui;

public sealed class RenderManager : IEnumerable<IRenderable>
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

    public List<IRenderSource> Sources { get; set; } = [];

    public IEnumerator<IRenderable> GetEnumerator()
    {
        var sorted = new List<IRenderSource>(Sources);

        sorted.Sort(Comparer.Instance);

        foreach (IRenderSource renderSource in sorted)
        {
            foreach (IRenderable renderable in renderSource.Render())
            {
                yield return renderable;
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}