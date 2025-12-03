using SCENeo.Utils;

namespace SCENeo.UI;

public sealed class Stretcher : UIBaseImage
{
    public enum Scaling
    {
        None,
        Stretch,
        Slide,
        Hide,
    }

    private bool _update = false;

    private int _scaleWidth;

    private Scaling _textScaling = Scaling.None;

    public Stretcher() : base() { }

    public Stretcher(IRenderable source) 
        : base(source.Width, source.Height)
    {
        Source = source;
    }

    #region Properties

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

    public override int Width { get { return Source == null ? 0 : Source.Width * ScaleWidth; } }

    #endregion

    public override Grid2DView<Pixel> Render()
    {
        if (!Bake || !IsBaked || _update)
        {
            Update();

            IsBaked = true;
            _update = false;
        }

        return _source;
    }

    private void Update()
    {
        if (Source == null)
        {
            throw new NullReferenceException("Source was null.");
        }

        IView<Pixel> view = Source.Render();

        if (_source.Dimensions != this.Dimensions())
        {
            _source.CleanResize(Width, view.Height);
        }

        for (int y = 0; y < view.Height; y++)
        {
            for (int x = 0; x < view.Width; x++)
            {
                BufferLoad(view, x, y);
            }
        }
    }

    private void BufferLoad(IView<Pixel> view, int x, int y)
    {
        switch (TextScaling)
        {
        case Scaling.None:
            _source[x * ScaleWidth, y] = view[x, y];
            for (int i = 1; i < ScaleWidth; i++)
                _source[x * ScaleWidth + i, y] = new Pixel(view[x, y].FgColor, view[x, y].BgColor);
            break;
        case Scaling.Stretch:
            for (int i = 0; i < ScaleWidth; i++)
                _source[x * ScaleWidth + i, y] = view[x, y];
            break;
        case Scaling.Slide:
            for (int i = 0; i < ScaleWidth; i++)
                _source[x + view.Width * i, y] = view[x, y];
            break;
        case Scaling.Hide:
            for (int i = 0; i < ScaleWidth; i++)
                _source[x * ScaleWidth + i, y] = new Pixel(view[x, y].FgColor, view[x, y].BgColor);
            break;
        }
    }
}
