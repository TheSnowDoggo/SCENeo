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

    public bool Horizontal
    {
        get { return Mode is FlowMode.LeftToRight or FlowMode.RightToLeft; }
    }

    #endregion

    public int Fill()
    {
        int end = FillEnd();

        return Math.Clamp((int)Math.Round(Value.Unlerp(Min, Max).Lerp(0, end)), 0, end);
    }

    public int FillEnd()
    {
        return Horizontal ? Width : Height;
    }

    public void Resize(int width, int height)
    {
        _source.Resize(width, height);
        _update = true;
    }

    public override IView<Pixel> Render()
    {
        if (_update || _valueUpdate)
        {
            Update();
        }

        return _source.AsView();
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

        switch (Mode)
        {
        case FlowMode.LeftToRight:
            _source.Fill(FillPixel, 0, 0, fill, Height);
            _source.Fill(BackPixel, fill, 0, Width, Height);
            break;
        case FlowMode.TopToBottom:
            _source.Fill(FillPixel, 0, 0, Width, fill);
            _source.Fill(BackPixel, 0, fill, Width, Height);
            break;
        case FlowMode.RightToLeft:
            _source.Fill(BackPixel, 0, 0, Width - fill, Height);
            _source.Fill(FillPixel, Width - fill, 0, Width, Height);
            break;
        case FlowMode.BottomToTop:
            _source.Fill(BackPixel, 0, 0, Width, Height - fill);
            _source.Fill(FillPixel, 0, Height - fill, Width, Height);
            break;
        }

        _update      = false;
        _valueUpdate = false;
    }
}