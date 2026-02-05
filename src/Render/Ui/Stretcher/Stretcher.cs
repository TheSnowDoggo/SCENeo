namespace SCENeo.Ui;

public sealed class Stretcher : UiBase, IRenderable
{
    

    private readonly Image _buffer = new Image();

    private bool _update = false;

    private int _scaleWidth;

    private StretcherScaling _textScaling = StretcherScaling.None;

    public Stretcher() { }

    private IRenderable _source;

    public IRenderable Source
    {
        get { return _source; }
        set { SCEUtils.ObserveSet(value, ref _source, ref _update); }
    }

    public int ScaleWidth
    {
        get {  return _scaleWidth; }
        set
        {
            if (value < 1)
            {
                throw new NotImplementedException("Scale width cannot be negative or zero.");
            }

            _scaleWidth = value;
        }
    }

    public StretcherScaling Scaling
    {
        get { return _textScaling; }
        set { SCEUtils.ObserveSet(value, ref _textScaling, ref _update); }
    }

    public bool Bake { get; set; } = false;

    public bool IsBaked { get; set; } = false;

    public int Width { get { return Source == null ? 0 : Source.Width * ScaleWidth; } }
    public int Height { get { return Source == null ? 0 : Source.Height; } }

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
