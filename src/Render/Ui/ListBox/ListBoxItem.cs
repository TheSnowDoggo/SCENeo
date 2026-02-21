namespace SCENeo.Ui;

/// <summary>
/// A class representing an option.
/// </summary>
public class ListBoxItem : UpdateBase
{
    private ListBoxItem _inherited;

    /// <summary>
    /// Gets or sets the option to inherit from.
    /// </summary>
    public ListBoxItem Inherited
    {
        get => _inherited;
        set => ObserveSet(ref _inherited, value, Update);
    }

    private string _text;

    /// <summary>
    /// Gets or sets the override option text.
    /// </summary>
    public string Text
    {
        get => _text;
        set => ObserveSet(ref _text, value); 
    }

    private SCEColor? _selectedFgColor;

    /// <summary>
    /// Gets or sets the override selected foreground color.
    /// </summary>
    public SCEColor? SelectedFgColor
    {
        get => _selectedFgColor;
        set => ObserveSet(ref _selectedFgColor, value);
    }

    private SCEColor? _selectedBgColor;

    /// <summary>
    /// Gets or sets the override selected background color.
    /// </summary>
    public SCEColor? SelectedBgColor
    {
        get => _selectedBgColor;
        set => ObserveSet(ref _selectedBgColor, value);
    }

    private SCEColor? _unselectedFgColor;

    /// <summary>
    /// Gets or sets the override unselected foreground color.
    /// </summary>
    public SCEColor? UnselectedFgColor
    {
        get => _unselectedFgColor;
        set => ObserveSet(ref _unselectedFgColor, value);
    }

    private SCEColor? _unselectedBgColor;

    /// <summary>
    /// Gets or sets the override unselected background color.
    /// </summary>
    public SCEColor? UnselectedBgColor
    {
        get => _unselectedBgColor;
        set => ObserveSet(ref _unselectedBgColor, value);
    }

    private Anchor? _anchor;

    /// <summary>
    /// Gets or sets the override option text anchoring.
    /// </summary>
    public Anchor? Anchor
    {
        get => _anchor;
        set => ObserveSet(ref _anchor, value);
    }

    private bool? _fitToLength;

    /// <summary>
    /// Gets or sets whether the text should be fit to length.
    /// </summary>
    public bool? FitToLength
    {
        get => _fitToLength;
        set => ObserveSet(ref _fitToLength, value);
    }

    public string GetText()
    {
        return Text ?? Inherited?.GetText() ?? string.Empty;
    }

    public SCEColor GetSelectedFgColor()
    {
        return SelectedFgColor ?? Inherited?.GetSelectedFgColor() ?? SCEColor.Black;
    }

    public SCEColor GetSelectedBgColor()
    {
        return SelectedBgColor ?? Inherited?.GetSelectedBgColor() ?? SCEColor.Gray;
    }

    public SCEColor GetUnselectedFgColor()
    {
        return UnselectedFgColor ?? Inherited?.GetUnselectedFgColor() ?? SCEColor.Gray;
    }

    public SCEColor GetUnselectedBgColor()
    {
        return UnselectedBgColor ?? Inherited?.GetUnselectedBgColor() ?? SCEColor.Black;
    }

    public Anchor GetAnchor()
    {
        return Anchor ?? Inherited?.GetAnchor() ?? SCENeo.Anchor.None;
    }

    public bool GetFitToLength()
    {
        return FitToLength ?? Inherited?.GetFitToLength() ?? false;
    }

    public UpdateList<ListBoxItem> FromTemplate(IList<string> lines)
    {
        var list = new UpdateList<ListBoxItem>(lines.Count);

        foreach (string text in lines)
        {
            list.Add(FromTemplate(text));
        }

        return list;
    }

    public ListBoxItem FromTemplate(string text)
    {
        var inherit = this;
        return new ListBoxItem()
        {
            Inherited = inherit,
            Text = text,
        };
    }
}