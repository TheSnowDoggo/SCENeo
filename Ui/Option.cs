namespace SCENeo.Ui;

public sealed class Option
{
    public Option() { }

    public event EventHandler? OnUpdate;

    private string _text = string.Empty;

    public string Text
    {
        get { return _text; }
        set { Update(value, ref _text); }
    }

    private SCEColor _selectedFgColor = SCEColor.Black;

    public SCEColor SelectedFgColor
    {
        get { return _selectedFgColor; }
        set { Update(value, ref _selectedFgColor); }
    }

    private SCEColor _selectedBgColor = SCEColor.White;

    public SCEColor SelectedBgColor
    {
        get { return _selectedBgColor; }
        set { Update(value, ref _selectedBgColor); }
    }

    private SCEColor _unselectedFgColor = SCEColor.White;

    public SCEColor UnselectedFgColor
    {
        get { return _unselectedFgColor; }
        set { Update(value, ref _unselectedFgColor); }
    }

    private SCEColor _unselectedBgColor = SCEColor.Black;

    public SCEColor UnselectedBgColor
    {
        get { return _unselectedBgColor; }
        set { Update(value, ref _unselectedBgColor); }
    }

    private Anchor _anchor = Anchor.None;

    public Anchor Anchor
    {
        get { return _anchor; }
        set { Update(value, ref _anchor); }
    }

    public Action? OnChoose { get; set; }

    private void Update<T>(T value, ref T field)
    {
        if (EqualityComparer<T>.Default.Equals(value, field))
        {
            return;
        }

        field = value;

        OnUpdate?.Invoke(this, EventArgs.Empty);
    }
}