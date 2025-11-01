using SCENeo.UI;

namespace SCENeo.Node.Render;

public sealed class RenderChannel : UIBaseImage
{
    public Pixel BasePixel;

    public RenderChannel() : base() { }
    public RenderChannel(int width, int height) : base(width, height) { }
    public RenderChannel(Vec2I dimensions) : base(dimensions) { }

    public void Clear()
    {
        _source.Fill(BasePixel);
    }

    public void Load(Grid2DView<Pixel> view, Vec2I position)
    {
        _source.MergeMap(view, position);
    }
}