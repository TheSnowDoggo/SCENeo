namespace SCENeo.UI;

public sealed class Option
{
    private string _text = string.Empty;

    private SCEColor _selectedFgColor = SCEColor.Black;
    private SCEColor _selectedBgColor = SCEColor.White;

    private SCEColor _unselectedFgColor = SCEColor.White;
    private SCEColor _unselectedBgColor = SCEColor.Black;

    private Anchor _anchor = Anchor.None;

    public Option() { }

    public event EventHandler? OnUpdate;

    #region Properties

    public string Text
    {
        get { return _text; }
        set { Update(value, ref _text); }
    }

    public SCEColor SelectedFgColor
    {
        get { return _selectedFgColor; }
        set { Update(value, ref _selectedFgColor); }
    }

    public SCEColor SelectedBgColor
    {
        get { return _selectedBgColor; }
        set { Update(value, ref _selectedBgColor); }
    }

    public SCEColor UnselectedFgColor
    {
        get { return _unselectedFgColor; }
        set { Update(value, ref _unselectedFgColor); }
    }

    public SCEColor UnselectedBgColor
    {
        get { return _unselectedBgColor; }
        set { Update(value, ref _unselectedBgColor); }
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