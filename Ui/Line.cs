namespace SCENeo.Ui;

public class Line
{
    private string _text = string.Empty;

    private SCEColor _fgColor = SCEColor.Gray;

    private SCEColor _bgColor = SCEColor.Black;

    private Anchor _anchor = Anchor.None;

    public Line() { }

    public event EventHandler? OnUpdate;

    public string Text
    {
        get { return _text; }
        set { Update(value, ref _text); }
    }

    public SCEColor fgColor
    {
        get { return _fgColor; }
        set { Update(value, ref _fgColor); }
    }

    public SCEColor bgColor
    {
        get { return _bgColor; }
        set { Update(value, ref _bgColor); }
    }

    public Anchor Anchor
    {
        get { return _anchor; }
        set { Update(value, ref _anchor); }
    }

    private void Update<T>(T value, ref T field)
    {
        if (EqualityComparer<T>.Default.Equals(value, field)) return;

        field = value;

        OnUpdate?.Invoke(this, EventArgs.Empty);
    }
}