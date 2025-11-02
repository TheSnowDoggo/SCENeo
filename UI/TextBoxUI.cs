using System.Text;
using SCENeo.Utils;

namespace SCENeo.UI;

public sealed class TextBoxUI : UIBaseImage
{
    private bool _update = false;

    private Pixel _basePixel;

    private string _text = string.Empty;

    private ColorInfo _textColorInfo = new(SCEColor.Gray, SCEColor.Black);

    private Anchor _textAnchor = Anchor.None;

    private bool _textOverflow = false;

    public TextBoxUI() : base() { }
    public TextBoxUI(int width, int height) : base(width, height) { }
    public TextBoxUI(Vec2I dimensions) : base(dimensions) { }

    #region Properties

    public Pixel BasePixel
    {
        get { return _basePixel; }
        set { SCEUtils.ObserveSet(value, ref _basePixel, ref _update); }
    }

    public string Text
    {
        get { return _text; }
        set { SCEUtils.ObserveSet(value, ref _text, ref _update); }
    }

    public SCEColor TextFgColor
    {
        get { return _textColorInfo.ForegroundColor; }
        set { SCEUtils.ObserveSet(value, ref _textColorInfo.ForegroundColor, ref _update); }
    }

    public SCEColor TextBgColor
    {
        get { return _textColorInfo.BackgroundColor; }
        set { SCEUtils.ObserveSet(value, ref _textColorInfo.BackgroundColor, ref _update); }
    }

    public Anchor TextAnchor
    {
        get { return _textAnchor; }
        set { SCEUtils.ObserveSet(value, ref _textAnchor, ref _update); }
    }

    public bool TextOverflow
    {
        get { return _textOverflow; }
        set { SCEUtils.ObserveSet(value, ref _textOverflow, ref _update); }
    }

    #endregion

    private static bool IsNewline(char c)
    {
        return c is '\n' or '\f' or '\t';
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

            if (newLine || (TextOverflow && sb.Length == Width && (i == s.Length - 1 || !IsNewline(s[i + 1]))))
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

    protected override void Update()
    {
        if (!_update) return;

        _update = false;

        _source.Fill(BasePixel);

        var lines = SplitLines(Text);

        int top = TextAnchor.AnchorVertical(Height - lines.Count);

        int startY = Math.Max(top, 0);
        int endY = Math.Min(top + lines.Count, Height);

        for (int y = startY, i = startY - top; y < endY; y++, i++)
        {
            int x = TextAnchor.AnchorHorizontal(Width - lines[i].Length);

            _source.MapLine(lines[i], x, y, _textColorInfo);
        }
    }
}