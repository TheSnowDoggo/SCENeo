namespace SCENeo.Ui;

public sealed class VerticalSelector : IRenderable
{
    private readonly Image _buffer = new Image();

    private bool _update = false;

    private Option[] _options = [];

    private int _selected = 0;

    public VerticalSelector()
    {
    }

    public Option this[int option]
    {
        get
        {
            return _options[option];
        }
        set
        {
            if (value == _options[option])
            {
                return;
            }

            if (_options[option] != null)
            {
                _options[option].OnUpdate -= Line_OnUpdate;
            }

            if (value != null)
            {
                value.OnUpdate += Line_OnUpdate;
            }

            _options[option] = value!;

            _update = true;
        }
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
        get
        {
            return _height;
        }
        set
        {
            if (value == _height)
            {
                return;
            }

            _height = value;

            for (int i = _height; i < _options.Length; i++)
            {
                if (_options[i] != null)
                {
                    _options[i].OnUpdate -= Line_OnUpdate;
                }
            }

            Array.Resize(ref _options, _height);

            _update = true;
        }
    }

    private Pixel _basePixel = new Pixel(SCEColor.Gray, SCEColor.Black);

    public Pixel BasePixel
    {
        get { return _basePixel; }
        set { SCEUtils.ObserveSet(value, ref _basePixel, ref _update); }
    }

    private StackMode _stackMode = StackMode.TopDown;

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
        Selected = SCEMath.Mod(Selected + move, Height);
    }

    public void LimitMove(int move)
    {
        Selected = Math.Clamp(Selected + move, 0, Height - 1);
    }

    public IView<Pixel> Render()
    {
        if (_update)
        {
            Update();
        }

        return _buffer.AsReadonly();
    }

    private void Update()
    {
        if (_buffer.Width != Width || _buffer.Height != Height)
        {
            _buffer.CleanResize(Width, Height);
        }

        _buffer.Fill(BasePixel);

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

            _buffer.MapLine(option.Text, x, y, fgColor, bgColor);
        }

        _update = false;
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