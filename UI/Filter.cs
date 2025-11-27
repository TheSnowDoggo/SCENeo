using SCENeo.Utils;

namespace SCENeo.UI;

public sealed class Filter : UIModifier<IRenderable>
{
    private readonly Image _buffer;

    public Filter(IRenderable source) : base(source)
    {
        _buffer = new Image(source.Width, source.Height);
    }

    public Func<Pixel, Pixel>? FilterMode = null;

    public override IView<Pixel> Render()
    {
        IView<Pixel> view = _source.Render();

        if (FilterMode == null) return view;

        Vec2I dimensions = view.Dimensions();

        if (_buffer.Dimensions != dimensions)
        {
            _buffer.CleanResize(dimensions);
        }

        _buffer.Fill((x, y) => FilterMode.Invoke(view[x, y]));

        return (Grid2DView<Pixel>)_buffer;
    }
}