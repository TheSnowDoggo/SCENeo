using SCENeo.UI;

namespace SCENeo.Node;

public sealed class RenderChannel : UIBaseImage
{
    public Pixel BasePixel;

    public void Clear()
    {
        _data.Fill(BasePixel);
    }

    public void Load(Grid2DView<Pixel> view, Vec2I position)
    {
        _data.MergeMap(view, position);
    }
}