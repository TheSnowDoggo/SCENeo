namespace SCENeo.Ui;

public sealed class Filter : UiModifier<IRenderable>
{
    private readonly Image _buffer = new Image();

    public Filter() { }

    public Func<Pixel, Pixel> FilterMode;

    public override IView<Pixel> Render()
    {
        IView<Pixel> view = Source.Render();

        if (FilterMode == null)
        {
            return view;
        }

        if (_buffer.Width != Width || _buffer.Height != Height)
        {
            _buffer.CleanResize(Width, Height);
        }

        _buffer.Fill((x, y) => FilterMode.Invoke(view[x, y]));

        return _buffer.AsReadonly();
    }
}