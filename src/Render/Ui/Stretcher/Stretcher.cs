namespace SCENeo.Ui;

public sealed class Stretcher : UiBase, IRenderable
{
    private readonly Image _buffer = new Image();

    private bool _update;

    private IRenderable _source;

    public IRenderable Source
    {
        get => _source;
        set => ObserveSet(ref _source, value, ref _update);
    }

    private int _scaleWidth;
    
    public int ScaleWidth
    {
        get => _scaleWidth;
        set
        {
            if (value < 1)
            {
                throw new NotImplementedException("Scale width cannot be negative or zero.");
            }

            _scaleWidth = value;
        }
    }

    private StretcherScaling _textScaling;

    public StretcherScaling Scaling
    {
        get => _textScaling;
        set => ObserveSet(ref _textScaling, value, ref _update);
    }

    public bool Bake { get; set; }
    public bool IsBaked { get; set; }

    public int Width => Source == null ? 0 : Source.Width * ScaleWidth;
    public int Height =>  Source?.Height ?? 0;

    public IView<Pixel> Render()
    {
        if (!Bake || !IsBaked || _update)
        {
            Update();
        }

        return _buffer.AsReadonly();
    }

    private void Update()
    {
        if (Source == null)
        {
            throw new NullReferenceException("Source was null.");
        }

        if (_buffer.Width != Width || _buffer.Height != Height)
        {
            _buffer.CleanResize(Width, Height);
        }

        IView<Pixel> view = Source.Render();

        for (int y = 0; y < view.Height; y++)
        {
            for (int x = 0; x < view.Width; x++)
            {
                BufferLoad(view, x, y);
            }
        }

        IsBaked = true;
        _update = false;
    }

    private void BufferLoad(IView<Pixel> view, int x, int y)
    {
        switch (Scaling)
        {
        case StretcherScaling.None:
            _buffer[x * ScaleWidth, y] = view[x, y];
            for (int i = 1; i < ScaleWidth; i++)
            {
                _buffer[x * ScaleWidth + i, y] = new Pixel(view[x, y].FgColor, view[x, y].BgColor);
            }
            break;
        case StretcherScaling.Stretch:
            for (int i = 0; i < ScaleWidth; i++)
            {
                _buffer[x * ScaleWidth + i, y] = view[x, y];
            }
            break;
        case StretcherScaling.Slide:
            for (int i = 0; i < ScaleWidth; i++)
            {
                _buffer[x + view.Width * i, y] = view[x, y];
            }
            break;
        case StretcherScaling.Hide:
            for (int i = 0; i < ScaleWidth; i++)
            {
                _buffer[x * ScaleWidth + i, y] = new Pixel(view[x, y].FgColor, view[x, y].BgColor);
            }
            break;
        }
    }
}
