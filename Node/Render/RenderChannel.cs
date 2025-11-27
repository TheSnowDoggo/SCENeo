using SCENeo.UI;

namespace SCENeo.Node.Render;

public sealed class RenderChannel : UIBaseImage
{
    public RenderChannel() : base() { }
    public RenderChannel(int width, int height) : base(width, height) { }
    public RenderChannel(Vec2I dimensions) : base(dimensions) { }

    #region Properties

    public Pixel BasePixel { get; set; }

    #endregion

    public void Clear()
    {
        _source.Fill(BasePixel);
    }

    public void Load(IView<Pixel> view, Vec2I position)
    {
        _source.MergeMap(view, position);
    }
}