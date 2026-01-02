namespace SCENeo.Ui;

public sealed class ProgressBar : IRenderable
{
    public enum FlowMode
    {
        LeftToRight,
        RightToLeft,
        TopToBottom,
        BottomToTop,
    }

    private readonly Image _buffer = new Image();

    private bool _update = false;

    private bool _valueUpdate = false;

    private int _lastFill = -1;

    public ProgressBar()
    {
    }

    public bool Visible { get; set; } = true;
    public Vec2I Offset { get; set; }
    public int ZIndex { get; set; }
    public Anchor Anchor { get; set; }

    private int _width;

    public int Width
    {
        get { return _width; }
        set { SCEUtils.ObserveSet(value, ref _width, ref _update); }
    }

    private int _height;

    public int Height
    {
        get { return _height; }
        set { SCEUtils.ObserveSet(value, ref _height, ref _update); }
    }

    private FlowMode _mode = FlowMode.LeftToRight;

    public FlowMode Mode
    {
        get { return _mode; }
        set { SCEUtils.ObserveSet(value, ref _mode, ref _update); }
    }

    private double _min = 0;

    public double Min
    {
        get { return _min; }
        set { SCEUtils.ObserveSet(value, ref _min, ref _valueUpdate); }
    }

    private double _max = 0;

    public double Max
    {
        get { return _max; }
        set { SCEUtils.ObserveSet(value, ref _max, ref _valueUpdate); }
    }

    private double _value = 0;

    public double Value
    {
        get { return _value; }
        set { SCEUtils.ObserveSet(value, ref _value, ref _valueUpdate); }
    }

    private Pixel _fillPixel = new Pixel(SCEColor.Green);

    public Pixel FillPixel
    {
        get { return _fillPixel; }
        set { SCEUtils.ObserveSet(value, ref _fillPixel, ref _update); }
    }

    private Pixel _backPixel = new Pixel(SCEColor.DarkGray);

    public Pixel BackPixel
    {
        get { return _backPixel; }
        set { SCEUtils.ObserveSet(value, ref _backPixel, ref _update); }
    }

    public bool Horizontal
    {
        get { return Mode is FlowMode.LeftToRight or FlowMode.RightToLeft; }
    }

    public int Fill()
    {
        int end = FillEnd();

        return Math.Clamp((int)Math.Round(Value.Unlerp(Min, Max).Lerp(0, end)), 0, end);
    }

    public int FillEnd()
    {
        return Horizontal ? Width : Height;
    }

    public IView<Pixel> Render()
    {
        if (_update || _valueUpdate)
        {
            Update();
        }

        return _buffer.AsView();
    }

    private void Update()
    {
        int fill = Fill();

        if (!_update && _valueUpdate && fill == _lastFill)
        {
            _valueUpdate = false;
            return;
        }

        _lastFill = fill;

        if (_buffer.Width != Width || _buffer.Height != Height)
        {
            _buffer.CleanResize(Width, Height);
        }

        switch (Mode)
        {
        case FlowMode.LeftToRight:
            _buffer.Fill(FillPixel, 0, 0, fill, Height);
            _buffer.Fill(BackPixel, fill, 0, Width, Height);
            break;
        case FlowMode.TopToBottom:
            _buffer.Fill(FillPixel, 0, 0, Width, fill);
            _buffer.Fill(BackPixel, 0, fill, Width, Height);
            break;
        case FlowMode.RightToLeft:
            _buffer.Fill(BackPixel, 0, 0, Width - fill, Height);
            _buffer.Fill(FillPixel, Width - fill, 0, Width, Height);
            break;
        case FlowMode.BottomToTop:
            _buffer.Fill(BackPixel, 0, 0, Width, Height - fill);
            _buffer.Fill(FillPixel, 0, Height - fill, Width, Height);
            break;
        }

        _update      = false;
        _valueUpdate = false;
    }
}