using SCENeo.Utils;

namespace SCENeo.UI;

public sealed class Filter<T> : UIModifier<T>
    where T : IRenderable, IDimensioned
{
    private readonly Image _buffer;

    public Filter(T source) : base(source)
    {
        _buffer = new Image(source.Dimensions());
    }

    public Func<Pixel, Pixel>? FilterMode = null;

    public override Grid2DView<Pixel> Render()
    {
        Grid2DView<Pixel> view = _source.Render();

        if (FilterMode == null) return view;

        if (_buffer.Dimensions != view.Dimensions)
        {
            _buffer.CleanResize(view.Dimensions);
        }

        _buffer.Fill((x, y) => FilterMode.Invoke(view[x, y]));

        return _buffer;
    }
}