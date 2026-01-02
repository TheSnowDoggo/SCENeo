using System.Text;

namespace SCENeo.Ui;

public sealed class TextBox : IRenderable
{
    public enum Wrapping
    {
        None,
        Character,
        Word,
    }

    private readonly Image _buffer = [];

    private bool _update = false;

    public TextBox()
    {
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
        get { return _height; }
        set { SCEUtils.ObserveSet(value, ref _height, ref _update); }
    }

    private Pixel _basePixel;

    public Pixel BasePixel
    {
        get { return _basePixel; }
        set { SCEUtils.ObserveSet(value, ref _basePixel, ref _update); }
    }

    private string _text = string.Empty;

    public string Text
    {
        get { return _text; }
        set { SCEUtils.ObserveSet(value, ref _text, ref _update); }
    }

    private SCEColor _textFgColor = SCEColor.Gray;

    public SCEColor TextFgColor
    {
        get { return _textFgColor; }
        set { SCEUtils.ObserveSet(value, ref _textFgColor, ref _update); }
    }

    private SCEColor _textBgColor = SCEColor.Black;

    public SCEColor TextBgColor
    {
        get { return _textBgColor; }
        set { SCEUtils.ObserveSet(value, ref _textBgColor, ref _update); }
    }

    private Anchor _textAnchor = Anchor.None;

    public Anchor TextAnchor
    {
        get { return _textAnchor; }
        set { SCEUtils.ObserveSet(value, ref _textAnchor, ref _update); }
    }

    private Wrapping _textWrapping = Wrapping.None;

    public Wrapping TextWrapping
    {
        get { return _textWrapping; }
        set { SCEUtils.ObserveSet(value, ref _textWrapping, ref _update); }
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

            if (newline || (sb.Length == Width && (i == Text.Length - 1 || Text[i + 1] != '\n')))
            {
                list.Add(sb.ToString());
                sb.Clear();
            }
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
                continue;
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

    private IReadOnlyList<string> GetLines()
    {
        return TextWrapping switch
        {
            Wrapping.None      => Text.Split('\n'),
            Wrapping.Character => CharacterSplitLines(),
            Wrapping.Word      => WordSplitLines(),
            _ => throw new Exception($"Impossible wrapping mode {TextWrapping}")
        };
    }
}