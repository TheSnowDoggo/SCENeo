namespace SCENeo.UI;

public class Line
{
    private string _text = string.Empty;

    private ColorInfo _colors = new ColorInfo(SCEColor.Transparent, SCEColor.Transparent);

    private Anchor _anchor = Anchor.None;

    public Line() { }

    public event EventHandler? OnUpdate;

    public string Text
    {
        get { return _text; }
        set { Update(value, ref _text); }
    }

    public SCEColor ForegroundColor
    {
        get { return _colors.ForegroundColor; }
        set { Update(value, ref _colors.ForegroundColor); }
    }

    public SCEColor BackgroundColor
    {
        get { return _colors.BackgroundColor; }
        set { Update(value, ref _colors.BackgroundColor); }
    }

    public ColorInfo Colors 
    { 
        get { return _colors; }
        set { Update(value, ref _colors); }
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