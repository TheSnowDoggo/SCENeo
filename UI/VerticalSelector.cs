using Microsoft.VisualBasic.FileIO;
using SCENeo.Utils;

namespace SCENeo.UI;

public sealed class VerticalSelector : UIBaseImage
{
    private bool _update = false;

    private Option[] _options = [];

    private Pixel _basePixel = new Pixel(SCEColor.Gray, SCEColor.Black);

    private StackMode _stackMode = StackMode.TopDown;

    private int _selected = 0;

    public VerticalSelector() : base() { }
    public VerticalSelector(int width, int height) : base(width, height)
    {
        _options = new Option[height];
    }
    public VerticalSelector(Vec2I dimensions) : base(dimensions)
    {
        _options = new Option[dimensions.Y];
    }

    #region Properties

    public Option this[int option]
    {
        get { return _options[option]; }
        set
        {
            if (_options[option] != null)
            {
                _options[option].OnUpdate -= Line_OnUpdate;
            }

            _options[option] = value;

            _options[option].OnUpdate += Line_OnUpdate;

            _update = true;
        }
    }

    public Pixel BasePixel
    {
        get { return _basePixel; }
        set { SCEUtils.ObserveSet(value, ref _basePixel, ref _update); }
    }

    public StackMode StackMode
    {
        get { return _stackMode; }
        set { SCEUtils.ObserveSet(value, ref _stackMode, ref _update); }
    }

    public int Selected
    {
        get { return _selected; }
        set { SCEUtils.ObserveSet(value, ref _selected, ref _update); }
    }

    #endregion

    public override void Resize(int width, int height)
    {
        base.Resize(width, height);

        _update = true;

        for (int i = height; i < _options.Length; i++)
        {
            if (_options[i] == null) continue;

            _options[i].OnUpdate -= Line_OnUpdate;
        }

        Array.Resize(ref _options, height);
    }

    public bool Select()
    {
        if (_selected < 0 || _selected >= _options.Length)
        {
            return false;
        }

        _options[_selected].OnChoose?.Invoke();

        return true;
    }

    public void WrapMove(int move)
    {
        Selected = SCEUtils.Mod(Selected + move, Height);
    }

    public void LimitMove(int move)
    {
        Selected = Math.Clamp(Selected + move, 0, Height - 1);
    }

    protected override void Update()
    {
        if (!_update) return;

        _update = false;

        _source.Fill(BasePixel);

        for (int i = 0; i < _options.Length; i++)
        {
            Option? option = _options[i];

            if (option == null) continue;
            if (option.Text == null) continue;

            int x = option.Anchor.AnchorHorizontal(Width - option.Text.Length);
            int y = TranslateY(i);

            bool selected = i == _selected;

            SCEColor fgColor = selected ? option.SelectedFgColor : option.UnselectedFgColor;
            SCEColor bgColor = selected ? option.SelectedBgColor : option.UnselectedBgColor;

            _source.MapLine(option.Text, x, y, fgColor, bgColor);
        }
    }

    private void Line_OnUpdate(object? sender, EventArgs args)
    {
        _update = true;
    }

    private int TranslateY(int i)
    {
        return StackMode switch
        {
            StackMode.TopDown  => i,
            StackMode.BottomUp => Height - i - 1,
            _ => throw new NotImplementedException($"Unimplemented stack mode {StackMode}.")
        };
    }
}