namespace SCENeo.Ui;

/// <summary>
/// A UI control representing a vertical set of lines.
/// </summary>
public sealed class List : UiBase, IRenderable
{
    private readonly Image _buffer = new Image();

    private bool _update;

    private UpdateList<ListItem> _items = [];

    public List()
    {
    }

    public UpdateList<ListItem> Items
    {
        get => _items;
        set => ObserveSet(ref _items, value, ref _update, Lines_OnUpdate);
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

    /// <summary>
    /// Gets or sets the base pixel.
    /// </summary>
    public Pixel BasePixel
    {
        get => _basePixel;
        set => ObserveSet(ref _basePixel, value, ref _update);
    }

    private StackMode _stackMode = StackMode.TopDown;

    /// <summary>
    /// Gets or sets the stack mode.
    /// </summary>
    public StackMode StackMode
    {
        get => _stackMode;
        set => ObserveSet(ref _stackMode, value, ref _update);
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

        for (int y = 0; y < Height; y++)
        {
            int index = TranslateIndex(y) + Scroll;

            if (index < 0 || index >= Items.Count)
            {
                ClearLine(y);
                continue;
            }

            ListItem line = Items[index];

            if (line == null)
            {
                ClearLine(y);
                continue;
            }

            string text      = line.GetText();
            SCEColor fgColor = line.GetFgColor();
            SCEColor bgColor = line.GetBgColor();
            Anchor anchor    = line.GetAnchor();

            if (line.GetFitToLength())
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

    private int TranslateIndex(int i)
    {
        return StackMode switch
        {
            StackMode.TopDown  => i,
            StackMode.BottomUp => Height - i - 1,
            _ => throw new NotImplementedException($"Unimplemented stack mode {StackMode}.")
        };
    }

    private void Lines_OnUpdate()
    {
        _update = true;
    }
}