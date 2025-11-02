using SCENeo.Utils;

namespace SCENeo.UI;

public sealed class LineRenderer : UIBaseImage
{
    private bool _update = false;

    private Line[] _lines = [];

    private Pixel _basePixel = new Pixel(SCEColor.Gray, SCEColor.Black);

    private StackMode _stackMode = StackMode.TopDown;

    public LineRenderer() : base() { }
    public LineRenderer(int width, int height) : base(width, height) 
    {
        _lines = new Line[height];
    }
    public LineRenderer(Vec2I dimensions) : base(dimensions) 
    {
        _lines = new Line[dimensions.Y];
    }

    public Line this[int line]
    {
        get { return _lines[line]; }
        set
        {
            if (_lines[line] != null)
            {
                _lines[line].OnUpdate -= Line_OnUpdate;
            }

            _lines[line] = value;

            _lines[line].OnUpdate += Line_OnUpdate;

            _update = true;
        }
    }

    public Pixel BasePixel
    {
        get { return _basePixel; }
        set { SCEUtils.ObserveSet(value, ref _basePixel, ref _update); }
    }

    public StackMode StackMode
    {
        get { return _stackMode; }
        set { SCEUtils.ObserveSet(value, ref _stackMode, ref _update); }
    }

    public override void Resize(int width, int height)
    {
        base.Resize(width, height);

        _update = true;

        for (int i = height; i < _lines.Length; i++)
        {
            if (_lines[i] == null) continue;

            _lines[i].OnUpdate -= Line_OnUpdate;
        }

        Array.Resize(ref _lines, height);
    }

    protected override void Update()
    {
        if (!_update) return;

        _update = false;

        _source.Fill(BasePixel);

        for (int i = 0; i < _lines.Length; i++)
        {
            if (_lines[i] == null) continue;
            if (_lines[i].Text == null) continue;

            int x = _lines[i].Anchor.AnchorHorizontal(Width - _lines[i].Text.Length);
            int y = TranslateY(i);

            _source.MapLine(_lines[i].Text, x, y, _lines[i].Colors);
        }
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