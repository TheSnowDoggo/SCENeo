using SCENeo.Utils;

namespace SCENeo.UI;

public sealed class LineRenderer : UIBaseImage, IResizeable
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

    public void Resize(int width, int height)
    {
        _source.Resize(width, height);

        _update = true;

        for (int i = height; i < _lines.Length; i++)
        {
            if (_lines[i] == null) continue;

            _lines[i].OnUpdate -= Line_OnUpdate;
        }

        Array.Resize(ref _lines, height);
    }

    public override IView<Pixel> Render()
    {
        if (_update)
        {
            Update();
            _update = false;
        }

        return _source.AsView();
    }

    private void Update()
    {
        _source.Fill(BasePixel);

        for (int i = 0; i < _lines.Length; i++)
        {
            Line line = _lines[i];

            if (line == null)
            {
                continue;
            }

            if (line.Text == null)
            {
                continue;
            }

            int x = line.Anchor.AnchorHorizontal(Width - line.Text.Length);
            int y = TranslateY(i);

            _source.MapLine(line.Text, x, y, line.fgColor, line.bgColor);
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