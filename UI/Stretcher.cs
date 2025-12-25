namespace SCENeo.Ui;

public sealed class Stretcher : IRenderable
{
    public enum Scaling
    {
        None,
        Stretch,
        Slide,
        Hide,
    }

    private readonly Image _buffer = new Image();

    private bool _update = false;

    private int _scaleWidth;

    private Scaling _textScaling = Scaling.None;

    public Stretcher()
    {
    }

    public bool Enabled { get; set; } = true;
    public Vec2I Offset { get; set; }
    public int ZOffset { get; set; }
    public Anchor Anchor { get; set; }

    public IRenderable? Source { get; set; }

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

    public Scaling TextScaling
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

        return _buffer.AsView();
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
        switch (TextScaling)
        {
        case Scaling.None:
            _buffer[x * ScaleWidth, y] = view[x, y];
            for (int i = 1; i < ScaleWidth; i++)
            {
                _buffer[x * ScaleWidth + i, y] = new Pixel(view[x, y].FgColor, view[x, y].BgColor);
            }
            break;
        case Scaling.Stretch:
            for (int i = 0; i < ScaleWidth; i++)
            {
                _buffer[x * ScaleWidth + i, y] = view[x, y];
            }
            break;
        case Scaling.Slide:
            for (int i = 0; i < ScaleWidth; i++)
            {
                _buffer[x + view.Width * i, y] = view[x, y];
            }
            break;
        case Scaling.Hide:
            for (int i = 0; i < ScaleWidth; i++)
            {
                _buffer[x * ScaleWidth + i, y] = new Pixel(view[x, y].FgColor, view[x, y].BgColor);
            }
            break;
        }
    }
}
