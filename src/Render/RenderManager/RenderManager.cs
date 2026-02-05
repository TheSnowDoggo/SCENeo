using System.Collections;

namespace SCENeo.Ui;

public sealed partial class RenderManager : IEnumerable<IRenderable>
{
    private sealed class Comparer : IComparer<IRenderSource>
    {
        public int Compare(IRenderSource a, IRenderSource b)
        {
            return a != null && b != null ? a.Priority.CompareTo(b.Priority) : 0;
        }
    }

    private static readonly Comparer DefaultComparer = new();

    public List<IRenderSource> Sources { get; set; } = [];

    public IEnumerator<IRenderable> GetEnumerator()
    {
        var sorted = new List<IRenderSource>(Sources);

        sorted.Sort(DefaultComparer);

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