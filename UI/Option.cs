namespace SCENeo.UI;

public sealed class Option
{
    private string _text = string.Empty;

    private ColorInfo _selectedColors = new ColorInfo(SCEColor.White, SCEColor.Black);

    private ColorInfo _unselectedColors = new ColorInfo(SCEColor.Black, SCEColor.White);

    private Anchor _anchor = Anchor.None;

    public Option() { }

    public event EventHandler? OnUpdate;

    #region Properties

    public string Text
    {
        get { return _text; }
        set { Update(value, ref _text); }
    }

    public ColorInfo SelectedColors
    {
        get { return _selectedColors; }
        set { Update(value, ref _selectedColors); }
    }

    public ColorInfo UnselectedColors
    {
        get { return _unselectedColors; }
        set { Update(value, ref _unselectedColors); }
    }

    public Anchor Anchor
    {
        get { return _anchor; }
        set { Update(value, ref _anchor); }
    }

    public Action? OnChoose { get; set; }

    #endregion

    private void Update<T>(T value, ref T field)
    {
        if (EqualityComparer<T>.Default.Equals(value, field)) return;

        field = value;

        OnUpdate?.Invoke(this, EventArgs.Empty);
    }
}