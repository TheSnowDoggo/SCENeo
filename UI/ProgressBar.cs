using SCENeo.Utils;

namespace SCENeo.UI;

public sealed class ProgressBar : UIBaseImage, IResizeable
{
    public enum FlowMode
    {
        LeftToRight,
        RightToLeft,
        TopToBottom,
        BottomToTop,
    }

    private bool _update = false;

    private bool _valueUpdate = false;

    private FlowMode _mode = FlowMode.LeftToRight;

    private double _min   = 0;

    private double _max   = 0;

    private double _value = 0;

    private Pixel _fillPixel = new Pixel(SCEColor.Green);

    private Pixel _backPixel = new Pixel(SCEColor.DarkGray);

    private int _lastFill = -1;

    public ProgressBar() : base() { }
    public ProgressBar(int width, int height) : base(width, height) { }
    public ProgressBar(Vec2I dimensions) : base(dimensions) { }

    #region Properties

    public FlowMode Mode
    {
        get { return _mode; }
        set { SCEUtils.ObserveSet(value, ref _mode, ref _update); }
    }

    public double Min
    {
        get { return _min; }
        set { SCEUtils.ObserveSet(value, ref _min, ref _valueUpdate); }
    }

    public double Max
    {
        get { return _max; }
        set { SCEUtils.ObserveSet(value, ref _max, ref _valueUpdate); }
    }

    public double Value
    {
        get { return _value; }
        set { SCEUtils.ObserveSet(value, ref _value, ref _valueUpdate); }
    }

    public Pixel FillPixel
    {
        get { return _fillPixel; }
        set { SCEUtils.ObserveSet(value, ref _fillPixel, ref _update); }
    }

    public Pixel BackPixel
    {
        get { return _backPixel; }
        set { SCEUtils.ObserveSet(value, ref _backPixel, ref _update); }
    }

    public int Fill
    {
        get
        {
            int end = FillEnd;
            return Math.Clamp((int)Math.Round(Value.Unlerp(Min, Max).Lerp(0, end)), 0, end);
        }
    }

    public int FillEnd
    {
        get { return Horizontal ? Width : Height; }
    }

    public bool Horizontal
    {
        get { return Mode is FlowMode.LeftToRight or FlowMode.RightToLeft; }
    }

    #endregion

    public void Resize(int width, int height)
    {
        _source.Resize(width, height);
        _update = true;
    }

    protected override void Update()
    {
        if (!_update && !_valueUpdate)
        {
            return;
        }

        int fill = Fill;

        if (!_update && _valueUpdate && fill == _lastFill)
        {
            return;
        }

        _lastFill = fill;

        switch (Mode)
        {
        case FlowMode.LeftToRight:
            _source.Fill(FillPixel, new Rect2DI(0   , 0, fill , Height));
            _source.Fill(BackPixel, new Rect2DI(fill, 0, Width, Height));
            break;
        case FlowMode.TopToBottom:
            _source.Fill(FillPixel, new Rect2DI(0, 0   , Width, fill  ));
            _source.Fill(BackPixel, new Rect2DI(0, fill, Width, Height));
            break;
        case FlowMode.RightToLeft:
            _source.Fill(BackPixel, new Rect2DI(0           , 0, Width - fill, Height));
            _source.Fill(FillPixel, new Rect2DI(Width - fill, 0, Width       , Height));
            break;
        case FlowMode.BottomToTop:
            _source.Fill(BackPixel, new Rect2DI(0,             0, Width, Height - fill));
            _source.Fill(FillPixel, new Rect2DI(0, Height - fill, Width, Height       ));
            break;
        }

        _update      = false;
        _valueUpdate = false;
    }
}