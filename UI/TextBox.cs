using System.Text;

namespace SCENeo.Ui;

public sealed class TextBox : IRenderable
{
    private readonly Image _buffer = new Image();

    private bool _update = false;

    public TextBox()
    {
    }

    public bool Enabled { get; set; } = true;
    public Vec2I Offset { get; set; }
    public int ZOffset { get; set; }
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

    private bool _textWrapping = false;

    public bool TextWrapping
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

        return _buffer.AsView();
    }

    private static bool IsNewline(char c)
    {
        return c is '\n' or '\f';
    }

    private List<string> SplitLines(string s)
    {
        var list = new List<string>();

        var sb = new StringBuilder();

        for (int i = 0; i < s.Length; i++)
        {
            bool newLine = IsNewline(s[i]);

            if (!newLine) 
            {
                sb.Append(s[i]);
            }

            if (newLine || (TextWrapping && sb.Length == Width && (i == s.Length - 1 || !IsNewline(s[i + 1]))))
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

    private void Update()
    {
        if (Width != _buffer.Width || Height != _buffer.Height)
        {
            _buffer.CleanResize(Width, Height);
        }

        _buffer.Fill(BasePixel);

        var lines = SplitLines(Text);

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
}