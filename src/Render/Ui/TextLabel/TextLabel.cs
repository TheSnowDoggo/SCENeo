namespace SCENeo.Ui;

/// <summary>
/// A UI control representing a text label.
/// </summary>
public sealed class TextLabel : UiBaseDimensioned, IRenderable
{
    private readonly Image _buffer = [];

    private Pixel _basePixel;

    /// <summary>
    /// Gets or sets the base pixel.
    /// </summary>
    public Pixel BasePixel
    {
        get => _basePixel;
        set => ObserveSet(ref _basePixel, value);
    }

    private string _text = string.Empty;

    /// <summary>
    /// Gets or sets the text.
    /// </summary>
    public string Text
    {
        get => _text;
        set => ObserveSet(ref _text, value);
    }

    private SCEColor _textFgColor = SCEColor.Gray;

    /// <summary>
    /// Gets or sets the text foreground color.
    /// </summary>
    public SCEColor TextFgColor
    {
        get => _textFgColor;
        set => ObserveSet(ref _textFgColor, value);
    }

    private SCEColor _textBgColor = SCEColor.Black;

    /// <summary>
    /// Gets or sets the text background color.
    /// </summary>
    public SCEColor TextBgColor
    {
        get => _textBgColor;
        set => ObserveSet(ref _textBgColor, value);
    }

    private Anchor _textAnchor;

    /// <summary>
    /// Gets or sets the text anchor alignment.
    /// </summary>
    public Anchor TextAnchor
    {
        get => _textAnchor;
        set => ObserveSet(ref _textAnchor, value);
    }

    private TextWrapping _textWrapping;

    /// <summary>
    /// Gets or sets the text wrapping mode.
    /// </summary>
    public TextWrapping TextWrapping
    {
        get => _textWrapping;
        set => ObserveSet(ref _textWrapping, value);
    }

    private bool _fitToLength;

    /// <summary>
    /// Gets or sets whether text should be fit to length on each line.
    /// </summary>
    public bool FitToLength
    {
        get => _fitToLength;
        set => ObserveSet(ref _fitToLength, value);
    }

    public IView<Pixel> Render()
    {
        if (!_update)
        {
            return _buffer;
        }
        
        _update = false;
        
        if (Width != _buffer.Width || Height != _buffer.Height)
        {
            _buffer.CleanResize(Width, Height);
        }

        _buffer.Fill(BasePixel);

        var textMapper = new TextMapper()
        {
            FitToLength = _fitToLength,
            Anchor = _textAnchor,
            Wrapping = _textWrapping,
            FgColor = _textFgColor,
            BgColor = _textBgColor,
        };
        
        textMapper.MapText(_buffer, _text);

        return _buffer;
    }
    
    /*
     * string[] lines = GetLines(Text, Width, TextWrapping);

        int top = TextAnchor.AnchorVertical(Height - lines.Length);

        int startY = Math.Max(top, 0);
        int endY = Math.Min(top + lines.Length, Height);

        for (int y = startY, i = startY - top; y < endY; y++, i++)
        {
            if (FitToLength)
            {
                _buffer.MapLine(Text.FitToLength(Width, TextAnchor), 0, y, _textFgColor, _textBgColor);
                continue;
            }

            int x = TextAnchor.AnchorHorizontal(Width - lines[i].Length);
            _buffer.MapLine(lines[i], x, y, _textFgColor, _textBgColor);
        }
     */
}