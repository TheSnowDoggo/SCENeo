namespace SCENeo.Ui;

/// <summary>
/// A class representing an option.
/// </summary>
public class ListBoxItem : IUpdate
{
    public event Action OnUpdate;

    public ListBoxItem()
    {
    }

    private ListBoxItem _inherited = null!;

    /// <summary>
    /// Gets or sets the option to inherit from.
    /// </summary>
    public ListBoxItem Inherited
    {
        get { return _inherited; }
        set
        {
            if (value == _inherited)
            {
                return;
            }

            if (_inherited != null)
            {
                _inherited.OnUpdate -= OnUpdate;
            }

            if (value != null)
            {
                value.OnUpdate += OnUpdate;
            }

            _inherited = value!;

            OnUpdate?.Invoke();
        }
    }

    private string _text;

    /// <summary>
    /// Gets or sets the override option text.
    /// </summary>
    public string Text
    {
        get { return _text; }
        set { Update(value, ref _text); }
    }

    private SCEColor? _selectedFgColor;

    /// <summary>
    /// Gets or sets the override selected foreground color.
    /// </summary>
    public SCEColor? SelectedFgColor
    {
        get => _selectedFgColor;
        set => Update(value, ref _selectedFgColor);
    }

    private SCEColor? _selectedBgColor;

    /// <summary>
    /// Gets or sets the override selected background color.
    /// </summary>
    public SCEColor? SelectedBgColor
    {
        get => _selectedBgColor;
        set => Update(value, ref _selectedBgColor);
    }

    private SCEColor? _unselectedFgColor;

    /// <summary>
    /// Gets or sets the override unselected foreground color.
    /// </summary>
    public SCEColor? UnselectedFgColor
    {
        get => _unselectedFgColor;
        set => Update(value, ref _unselectedFgColor);
    }

    private SCEColor? _unselectedBgColor;

    /// <summary>
    /// Gets or sets the override unselected background color.
    /// </summary>
    public SCEColor? UnselectedBgColor
    {
        get => _unselectedBgColor;
        set => Update(value, ref _unselectedBgColor);
    }

    private Anchor? _anchor;

    /// <summary>
    /// Gets or sets the override option text anchoring.
    /// </summary>
    public Anchor? Anchor
    {
        get => _anchor;
        set => Update(value, ref _anchor);
    }

    private bool? _fitToLength;

    /// <summary>
    /// Gets or sets whether the text should be fit to length.
    /// </summary>
    public bool? FitToLength
    {
        get => _fitToLength;
        set => Update(value, ref _fitToLength);
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

    public IEnumerable<ListBoxItem> SubOption(IEnumerable<string> optionText)
    {
        foreach (string text in optionText)
        {
            yield return new ListBoxItem()
            {
                Inherited = this,
                Text = text,
            };
        }
    }

    private void Update<T>(T value, ref T field)
    {
        if (!EqualityComparer<T>.Default.Equals(value, field))
        {
            field = value;
            OnUpdate?.Invoke();
        }
    }
}