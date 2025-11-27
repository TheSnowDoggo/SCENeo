using SCENeo.Utils;

namespace SCENeo.UI;

public sealed class Stretcher : UIModifier<IRenderable>
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

    public Stretcher(IRenderable source) : base(source) { }

    #region Properties

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

    public override int Width { get { return _source.Width * ScaleWidth; } }

    #endregion

    public override Grid2DView<Pixel> Render()
    {
        if (!Bake || !IsBaked || _update)
        {
            Update();

            IsBaked = true;
            _update = false;
        }

        return _buffer;
    }

    private void Update()
    {
        IView<Pixel> view = _source.Render();

        if (_buffer.Dimensions != this.Dimensions())
        {
            _buffer.CleanResize(Width, view.Height);
        }

        for (int y = 0; y < view.Height; y++)
        {
            for (int x = 0; x < view.Width; x++)
            {
                switch (TextScaling)
                {
                    case Scaling.None:
                        _buffer[x * ScaleWidth, y] = view[x, y];
                        for (int i = 1; i < ScaleWidth; i++)
                            _buffer[x * ScaleWidth + i, y] = new Pixel(view[x, y].FgColor, view[x, y].BgColor);
                        break;
                    case Scaling.Stretch:
                        for (int i = 0; i < ScaleWidth; i++)
                            _buffer[x * ScaleWidth + i, y] = view[x, y];
                        break;
                    case Scaling.Slide:
                        for (int i = 0; i < ScaleWidth; i++)
                            _buffer[x + view.Width * i, y] = view[x, y];
                        break;
                    case Scaling.Hide:
                        for (int i = 0; i < ScaleWidth; i++)
                            _buffer[x * ScaleWidth + i, y] = new Pixel(view[x, y].FgColor, view[x, y].BgColor);
                        break;
                }
            }
        }
    }
}
