namespace SCENeo;

/// <summary>
/// A readonly struct representing a single pixel.
/// </summary>
public readonly partial struct Pixel
    : IEquatable<Pixel>
{
    public const char ElementTransparent = '\0';
    public const char ElementOpaque = ' ';

    public Pixel(char element, SCEColor fgColor, SCEColor bgColor)
    {
        Element = element;
        FgColor = fgColor;
        BgColor = bgColor;
    }

    public Pixel(SCEColor fgColor, SCEColor bgColor)
        : this(' ', fgColor, bgColor)
    { 
    }

    public Pixel(SCEColor bgColor)
        : this(ElementOpaque, SCEColor.Black, bgColor)
    {
    }

    /// <summary>
    /// Gets or initializes the character element of this pixel.
    /// </summary>
    public char Element { get; init; }

    /// <summary>
    /// Gets or initializes the foreground color of this pixel.
    /// </summary>
    public SCEColor FgColor { get; init; }

    /// <summary>
    /// Gets or initializes the background color of this pixel.
    /// </summary>
    public SCEColor BgColor { get; init; }

    public static bool operator ==(Pixel p1, Pixel p2) => p1.Equals(p2);
    public static bool operator !=(Pixel p1, Pixel p2) => !p1.Equals(p2);

    /// <summary>
    /// Merges this pixel with the other pixel.
    /// </summary>
    /// <remarks>
    /// Used for pixel transparency.
    /// </remarks>
    /// <param name="other">Represents the pixel to be mapped on top of this pixel.</param>
    /// <returns>The merged pixel.</returns>
    public readonly Pixel Merge(Pixel other)
    {
        return new Pixel()
        {
            Element = Element.Merge(other.Element),
            FgColor = FgColor.Merge(other.FgColor),
            BgColor = BgColor.Merge(other.BgColor),
        };
    }

    /// <summary>
    /// Deconstructs this pixel.
    /// </summary>
    /// <param name="element">The character element.</param>
    /// <param name="fgColor">The foreground color.</param>
    /// <param name="bgColor">The background color.</param>
    public void Deconstruct(out char element, out SCEColor fgColor, out SCEColor bgColor)
    {
        element = Element;
        fgColor = FgColor;
        bgColor = BgColor;
    }

    public readonly bool Equals(Pixel other)
    {
        return Element == other.Element && FgColor == other.FgColor && BgColor == other.BgColor;
    }

    public readonly override bool Equals(object? obj)
    {
        return obj is Pixel pixel && Equals(pixel);
    }

    public readonly bool LooksLike(Pixel other)
    {
        if (BgColor != other.BgColor)
        {
            return false;
        }

        if (FgColor == other.FgColor && (Element == other.Element ||
            (char.IsControl(Element) && char.IsControl(other.Element))))
        {
            return true;
        }

        return false;
    }

    public readonly override int GetHashCode()
    {
        return HashCode.Combine(Element, FgColor, BgColor);
    }
}
