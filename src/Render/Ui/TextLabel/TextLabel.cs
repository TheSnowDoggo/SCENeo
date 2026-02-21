using System.Text;

namespace SCENeo.Ui;

/// <summary>
/// A UI control representing a text label.
/// </summary>
public sealed class TextLabel : UiBase, IRenderable
{
    private readonly Image _buffer = [];

    private bool _update;

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

    private Pixel _basePixel;

    /// <summary>
    /// Gets or sets the base pixel.
    /// </summary>
    public Pixel BasePixel
    {
        get => _basePixel;
        set => ObserveSet(ref _basePixel, value, ref _update);
    }

    private string _text = string.Empty;

    /// <summary>
    /// Gets or sets the text.
    /// </summary>
    public string Text
    {
        get => _text;
        set => ObserveSet(ref _text, value, ref _update);
    }

    private SCEColor _textFgColor = SCEColor.Gray;

    /// <summary>
    /// Gets or sets the text foreground color.
    /// </summary>
    public SCEColor TextFgColor
    {
        get => _textFgColor;
        set => ObserveSet(ref _textFgColor, value, ref _update);
    }

    private SCEColor _textBgColor = SCEColor.Black;

    /// <summary>
    /// Gets or sets the text background color.
    /// </summary>
    public SCEColor TextBgColor
    {
        get => _textBgColor;
        set => ObserveSet(ref _textBgColor, value, ref _update);
    }

    private Anchor _textAnchor;

    /// <summary>
    /// Gets or sets the text anchor alignment.
    /// </summary>
    public Anchor TextAnchor
    {
        get => _textAnchor;
        set => ObserveSet(ref _textAnchor, value, ref _update);
    }

    private TextWrapping _textWrapping;

    /// <summary>
    /// Gets or sets the text wrapping mode.
    /// </summary>
    public TextWrapping TextWrapping
    {
        get => _textWrapping;
        set => ObserveSet(ref _textWrapping, value, ref _update);
    }

    private bool _fitToLength;

    /// <summary>
    /// Gets or sets whether text should be fit to length on each line.
    /// </summary>
    public bool FitToLength
    {
        get => _fitToLength;
        set => ObserveSet(ref _fitToLength, value, ref _update);
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
        if (Width != _buffer.Width || Height != _buffer.Height)
        {
            _buffer.CleanResize(Width, Height);
        }

        _buffer.Fill(BasePixel);

        IReadOnlyList<string> lines = GetLines();

        int top = TextAnchor.AnchorVertical(Height - lines.Count);

        int startY = Math.Max(top, 0);
        int endY = Math.Min(top + lines.Count, Height);

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

        _update = false;
    }

    private List<string> CharacterSplitLines()
    {
        var list = new List<string>();

        var sb = new StringBuilder();

        for (int i = 0; i < Text.Length; i++)
        {
            bool newline = Text[i] == '\n';

            if (!newline)
            {
                sb.Append(Text[i]);
            }

            if (!newline && (sb.Length != Width || (i != Text.Length - 1 && Text[i + 1] == '\n')))
            {
                continue;
            }
            
            list.Add(sb.ToString());
            sb.Clear();
        }

        if (sb.Length != 0)
        {
            list.Add(sb.ToString());
        }

        return list;
    }

    private List<string> WordSplitLines()
    {
        var list = new List<string>();

        var wordBuilder = new StringBuilder();
        var lineBuilder = new StringBuilder();

        for (int i = 0; i < Text.Length; i++)
        {
            bool newLine = Text[i] == '\n';

            if (Text[i] != ' ' && !newLine)
            {
                wordBuilder.Append(Text[i]);
                
                if (i != Text.Length - 1)
                {
                    continue;
                }
            }

            if (newLine || lineBuilder.Length + wordBuilder.Length >= Width)
            {
                list.Add(lineBuilder.ToString());
                lineBuilder.Clear();
            }

            lineBuilder.Append(wordBuilder);
            wordBuilder.Clear();


            if (lineBuilder.Length < Width)
            {
                lineBuilder.Append(' ');
            }
        }

        if (lineBuilder.Length != 0)
        {
            list.Add(lineBuilder.ToString());
        }

        return list;
    }

    private IReadOnlyList<string> GetLines() => TextWrapping switch
    {
        TextWrapping.None      => Text.Split('\n'),
        TextWrapping.Character => CharacterSplitLines(),
        TextWrapping.Word      => WordSplitLines(),
        _ => throw new Exception($"Impossible wrapping mode {TextWrapping}")
    };
}