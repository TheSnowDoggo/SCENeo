namespace SCENeo.Ui;

/// <summary>
/// A UI control representinng a vertical set of options.
/// </summary>
public sealed partial class ListBox : UiBase, IRenderable
{
    private readonly Image _buffer = new Image();

    private bool _update = false;

    public ListBox()
    {
    }

    private UpdateList<Option> _options = [];

    /// <summary>
    /// Gets or sets the list of options.
    /// </summary>
    public UpdateList<Option> Options
    {
        get { return _options; }
        set
        {
            if (value == _options)
            {
                return;
            }

            if (_options != null)
            {
                _options.OnUpdate -= Option_OnUpdate;
            }

            if (value != null)
            {
                value.OnUpdate += Option_OnUpdate;
            }

            _options = value!;
            _update = true;
        }
    }

    private int _width;

    /// <summary>
    /// Gets or sets the width.
    /// </summary>
    public int Width
    {
        get { return _width; }
        set { SCEUtils.ObserveSet(value, ref _width, ref _update); }
    }

    private int _height;

    /// <summary>
    /// Gets or sets the height.
    /// </summary>
    public int Height
    {
        get { return _height; }
        set { SCEUtils.ObserveSet(value, ref _height, ref _update); }
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

    private int _selected = 0;

    public int Selected
    {
        get { return _selected; }
        set { SCEUtils.ObserveSet(value, ref _selected, ref _update); }
    }

    private int _scroll;

    /// <summary>
    /// Gets or sets the scroll.
    /// </summary>
    public int Scroll
    {
        get { return _scroll; }
        set { SCEUtils.ObserveSet(value, ref _scroll, ref _update); }
    }

    public void WrapMove(int move)
    {
        Selected = SCEMath.Mod(Selected + move, Height);
    }

    public void LimitMove(int move)
    {
        Selected = Math.Clamp(Selected + move, 0, Height - 1);
    }

    public void ScrollMove(int move)
    {
        Selected = Math.Clamp(Selected + move, 0, Options.Count - 1);

        Scroll = Math.Max(1 + Selected - Height, 0);
    }

    public IView<Pixel> Render()
    {
        if (_update)
        {
            Update();
        }

        return _buffer.AsReadonly();
    }

    public int TranslateIndex(int i)
    {
        return StackMode switch
        {
            StackMode.TopDown => i,
            StackMode.BottomUp => Height - i - 1,
            _ => throw new NotImplementedException($"Unimplemented stack mode {StackMode}.")
        };
    }

    private void Update()
    {
        if (_buffer.Width != Width || _buffer.Height != Height)
        {
            _buffer.CleanResize(Width, Height);
        }

        _buffer.Fill(BasePixel);

        for (int y = 0; y < Height; y++)
        {
            int index = TranslateIndex(y) + Scroll;

            if (index < 0 || index >= Options.Count)
            {
                ClearLine(y);
                continue;
            }

            Option option = Options[index];

            if (option == null)
            {
                ClearLine(y);
                continue;
            }

            string text = option.GetText();

            Anchor anchor = option.GetAnchor();

            bool selected = index == _selected;

            SCEColor fgColor = selected ? option.GetSelectedFgColor() : option.GetUnselectedFgColor();
            SCEColor bgColor = selected ? option.GetSelectedBgColor() : option.GetUnselectedBgColor();

            if (option.GetFitToLength())
            {
                _buffer.MapLine(text.FitToLength(Width, anchor), 0, y, fgColor, bgColor);
                continue;
            }

            int x = anchor.AnchorHorizontal(Width - text.Length);

            _buffer.MapLine(text, x, y, fgColor, bgColor);
        }

        _update = false;
    }

    private void ClearLine(int y)
    {
        for (int x = 0; x < Width; x++)
        {
            _buffer[x, y] = BasePixel;
        }
    }

    private void Option_OnUpdate()
    {
        _update = true;
    }
}