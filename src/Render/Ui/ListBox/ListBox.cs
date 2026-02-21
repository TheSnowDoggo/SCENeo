namespace SCENeo.Ui;

/// <summary>
/// A UI control representinng a vertical set of options.
/// </summary>
public sealed class ListBox : UiBase, IRenderable
{
    private readonly Image _buffer = new Image();

    private bool _update;

    public ListBox()
    {
    }

    private UpdateList<ListBoxItem> _items = [];

    /// <summary>
    /// Gets or sets the list of options.
    /// </summary>
    public UpdateList<ListBoxItem> Items
    {
        get => _items;
        set => ObserveSet(ref _items, value, ref _update, () => _update = true);
    }

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

    private Pixel _basePixel = new Pixel(SCEColor.Gray, SCEColor.Black);

    public Pixel BasePixel
    {
        get => _basePixel;
        set => ObserveSet(ref _basePixel, value, ref _update);
    }

    private StackMode _stackMode = StackMode.TopDown;

    public StackMode StackMode
    {
        get => _stackMode;
        set => ObserveSet(ref _stackMode, value, ref _update);
    }

    private int _selected;

    public int Selected
    {
        get => _selected;
        set => ObserveSet(ref _selected, value, ref _update);
    }

    private int _scroll;

    /// <summary>
    /// Gets or sets the scroll.
    /// </summary>
    public int Scroll
    {
        get => _scroll;
        set => ObserveSet(ref _scroll, value, ref _update);
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
        if (Items.Count == 0) return;

        Selected = Math.Clamp(Selected + move, 0, Items.Count - 1);

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

    public int TranslateIndex(int i) => StackMode switch
    {
        StackMode.TopDown  => i,
        StackMode.BottomUp => Height - i - 1,
        _ => throw new NotImplementedException($"Unimplemented stack mode {StackMode}."),
    };

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

            if (index < 0 || index >= Items.Count)
            {
                ClearLine(y);
                continue;
            }

            ListBoxItem option = Items[index];

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
}