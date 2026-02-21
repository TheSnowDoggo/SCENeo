namespace SCENeo.Ui;

public sealed class ProgressBar : UiBase, IRenderable
{
    private readonly Image _buffer = new Image();

    private bool _update = false;

    private bool _valueUpdate = false;

    private int _lastFill = -1;

    public ProgressBar() { }

    private int _width;

    /// <summary>
    /// Gets or sets the width.
    /// </summary>
    public int Width
    {
        get => _width;
        set => ObserveSet(ref _width, value, ref _update);
    }

    private int _height;

    /// <summary>
    /// Gets or sets the height.
    /// </summary>
    public int Height
    {
        get => _height;
        set => ObserveSet(ref _height, value, ref _update);
    }

    private ProgressBarFlow _mode;

    public ProgressBarFlow Mode
    {
        get => _mode;
        set => ObserveSet(ref _mode, value, ref _update);
    }

    private double _min;

    public double Min
    {
        get => _min;
        set => ObserveSet(ref _min, value, ref _update);
    }

    private double _max;

    public double Max
    {
        get => _max;
        set => ObserveSet(ref _max, value, ref _update);
    }

    private double _value;

    public double Value
    {
        get => _value;
        set => ObserveSet(ref _value, value, ref _update);
    }

    private Pixel _fillPixel = Pixel.Green;

    public Pixel FillPixel
    {
        get => _fillPixel;
        set => ObserveSet(ref _fillPixel, value, ref _update);
    }

    private Pixel _backPixel = Pixel.DarkGray;

    public Pixel BackPixel
    {
        get => _backPixel;
        set => ObserveSet(ref _backPixel, value, ref _update);
    }

    public bool Horizontal => Mode is ProgressBarFlow.LeftRight or ProgressBarFlow.RightLeft;

    public int FillEnd => Horizontal ? Width : Height;
    
    public int Fill()
    {
        return Math.Clamp((int)Math.Round(Value.Unlerp(Min, Max).Lerp(0, FillEnd)), 0, FillEnd);
    }

    public IView<Pixel> Render()
    {
        if (_update || _valueUpdate)
        {
            Update();
        }

        return _buffer.AsReadonly();
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
        case ProgressBarFlow.LeftRight:
            _buffer.Fill(FillPixel, 0, 0, fill, Height);
            _buffer.Fill(BackPixel, fill, 0, Width, Height);
            break;
        case ProgressBarFlow.TopBottom:
            _buffer.Fill(FillPixel, 0, 0, Width, fill);
            _buffer.Fill(BackPixel, 0, fill, Width, Height);
            break;
        case ProgressBarFlow.RightLeft:
            _buffer.Fill(BackPixel, 0, 0, Width - fill, Height);
            _buffer.Fill(FillPixel, Width - fill, 0, Width, Height);
            break;
        case ProgressBarFlow.BottomTop:
            _buffer.Fill(BackPixel, 0, 0, Width, Height - fill);
            _buffer.Fill(FillPixel, 0, Height - fill, Width, Height);
            break;
        }

        _update  = false;
        _valueUpdate = false;
    }
}