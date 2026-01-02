using System.Collections;

namespace SCENeo.Ui;

public sealed partial class RenderManager : IEnumerable<IRenderable>
{
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