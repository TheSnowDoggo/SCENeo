namespace SCENeo.Ui;

/// <summary>
/// A UI control representing a vertical set of lines.
/// </summary>
public sealed partial class List : UiBase, IRenderable
{
    private readonly Image _buffer = new Image();

    private bool _update = false;

    private UpdateList<Line> _lines = [];

    public List()
    {
    }

    public UpdateList<Line> Lines
    {
        get { return _lines; }
        set
        {
            if (value == _lines)
            {
                return;
            }

            if (_lines != null)
            {
                _lines.OnUpdate -= Lines_OnUpdate;
            }

            if (value != null)
            {
                value.OnUpdate += Lines_OnUpdate;
            }

            _lines = value!;
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

    /// <summary>
    /// Gets or sets the base pixel.
    /// </summary>
    public Pixel BasePixel
    {
        get { return _basePixel; }
        set { SCEUtils.ObserveSet(value, ref _basePixel, ref _update); }
    }

    private StackMode _stackMode = StackMode.TopDown;

    /// <summary>
    /// Gets or sets the stack mode.
    /// </summary>
    public StackMode StackMode
    {
        get { return _stackMode; }
        set { SCEUtils.ObserveSet(value, ref _stackMode, ref _update); }
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
            int index = TranslateIndex(y);

            if (index < 0 || index >= Lines.Count)
            {
                ClearLine(y);
                continue;
            }

            Line line = Lines[index];

            if (line == null || line.Text == null)
            {
                ClearLine(y);
                continue;
            }

            if (line.FitToLength)
            {
                _buffer.MapLine(line.Text.FitToLength(Width, line.Anchor), 0, y, line.FgColor, line.BgColor);

                continue;
            }

            int x = line.Anchor.AnchorHorizontal(Width - line.Text.Length);

            _buffer.MapLine(line.Text, x, y, line.FgColor, line.BgColor);
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
        } + Scroll;
    }

    private void Lines_OnUpdate()
    {
        _update = true;
    }
}