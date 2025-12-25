namespace SCENeo.Ui;

public sealed class LineRenderer : IRenderable
{
    private readonly Image _buffer = new Image();

    private bool _update = false;

    private Line[] _lines = [];

    public LineRenderer()
    {
    }

    public Line this[int line]
    {
        get 
        {
            return _lines[line];
        }
        set
        {
            if (_lines[line] == value)
            {
                return;
            }

            if (_lines[line] != null)
            {
                _lines[line].OnUpdate -= Line_OnUpdate;
            }

            if (value != null)
            {
                value.OnUpdate += Line_OnUpdate;
            }

            _lines[line] = value!;

            _update = true;
        }
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
        set
        {
            if (value == _height)
            {
                return;
            }

            _height = value;

            for (int i = _height; i < _lines.Length; i++)
            {
                if (_lines[i] != null)
                {
                    _lines[i].OnUpdate -= Line_OnUpdate;
                }
            }

            Array.Resize(ref _lines, _height);

            _update = true;
        }
    }

    private Pixel _basePixel = new Pixel(SCEColor.Gray, SCEColor.Black);

    public Pixel BasePixel
    {
        get { return _basePixel; }
        set { SCEUtils.ObserveSet(value, ref _basePixel, ref _update); }
    }

    private StackMode _stackMode = StackMode.TopDown;

    public StackMode StackMode
    {
        get { return _stackMode; }
        set { SCEUtils.ObserveSet(value, ref _stackMode, ref _update); }
    }

    public IView<Pixel> Render()
    {
        if (_update)
        {
            Update();
        }

        return _buffer.AsView();
    }

    private void Update()
    {
        if (_buffer.Width != Width || _buffer.Height != Height)
        {
            _buffer.CleanResize(Width, Height);
        }

        _buffer.Fill(BasePixel);

        for (int i = 0; i < _lines.Length; i++)
        {
            Line line = _lines[i];

            if (line == null || line.Text == null)
            {
                continue;
            }

            int x = line.Anchor.AnchorHorizontal(Width - line.Text.Length);
            int y = TranslateY(i);

            _buffer.MapLine(line.Text, x, y, line.fgColor, line.bgColor);
        }

        _update = false;
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