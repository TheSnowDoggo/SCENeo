using SCENeo;
namespace SCENeo.Ui;

/// <summary>
/// A class representing a single line in a <see cref="List"/>.
/// </summary>
public class ListItem : UpdateBase
{
    private ListItem _inherited;

    public ListItem Inherited
    {
        get => _inherited;
        set => ObserveSet(ref _inherited, value, Update);
    }

    private string _text;

    /// <summary>
    /// Gets or sets the text contents.
    /// </summary>
    public string Text
    {
        get => _text;
        set => ObserveSet(ref _text, value);
    }

    private SCEColor? _fgColor;

    /// <summary>
    /// Gets or sets the text foreground color.
    /// </summary>
    public SCEColor? FgColor
    {
        get => _fgColor;
        set => ObserveSet(ref _fgColor, value);
    }

    private SCEColor? _bgColor;

    /// <summary>
    /// Gets or sets the text background color.
    /// </summary>
    public SCEColor? BgColor
    {
        get => _bgColor;
        set => ObserveSet(ref _bgColor, value);
    }

    private Anchor? _anchor;

    /// <summary>
    /// Gets or sets the text anchor alignment.
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

    public SCEColor GetFgColor()
    {
        return FgColor ?? Inherited?.GetFgColor() ?? SCEColor.Gray;
    }

    public SCEColor GetBgColor()
    {
        return BgColor ?? Inherited?.GetBgColor() ?? SCEColor.Black;
    }

    public Anchor GetAnchor()
    {
        return Anchor ?? Inherited?.GetAnchor() ?? SCENeo.Anchor.None;
    }

    public bool GetFitToLength()
    {
        return FitToLength ?? Inherited?.GetFitToLength() ?? false;
    }

    public UpdateList<ListItem> FromTemplate(IList<string> lines)
    {
        var list = new UpdateList<ListItem>(lines.Count);

        foreach (string text in lines)
        {
            list.Add(FromTemplate(text));
        }

        return list;
    }

    public ListItem FromTemplate(string text)
    {
        var inherit = this;
        return new ListItem()
        {
            Inherited = inherit,
            Text = text,
        };
    }
}