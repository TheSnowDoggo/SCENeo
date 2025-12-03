using SCENeo.Utils;

namespace SCENeo;

public readonly struct Pixel
    : IEquatable<Pixel>
{
    public Pixel(char element, SCEColor fgColor, SCEColor bgColor)
    {
        Element = element;
        FgColor = fgColor;
        BgColor = bgColor;
    }

    public Pixel(char element, ColorInfo colors)
        : this(element, colors.FgColor, colors.BgColor)
    { }

    public Pixel(ColorInfo colors)
        : this(' ', colors)
    { }

    public Pixel(SCEColor fgColor, SCEColor bgColor)
        : this(' ', fgColor, bgColor)
    { }

    public Pixel(SCEColor bgColor)
        : this(' ', SCEColor.Black, bgColor)
    { }

    public char Element { get; init; }
    public SCEColor FgColor { get; init; }
    public SCEColor BgColor { get; init; }

    public static bool operator ==(Pixel p1, Pixel p2) => p1.Equals(p2);

    public static bool operator !=(Pixel p1, Pixel p2) => !p1.Equals(p2);

    public static Pixel Empty { get { return new Pixel(); } }

    public static Pixel Null { get { return new Pixel('\0', SCEColor.Transparent, SCEColor.Transparent); } }

    #region Presets

    public static Pixel Transparent => new Pixel(SCEColor.Transparent);
    public static Pixel Black => new Pixel(SCEColor.Black);
    public static Pixel DarkBlue => new Pixel(SCEColor.DarkBlue);
    public static Pixel DarkGreen => new Pixel(SCEColor.DarkGreen);
    public static Pixel DarkCyan => new Pixel(SCEColor.DarkCyan);
    public static Pixel DarkRed => new Pixel(SCEColor.DarkRed);
    public static Pixel DarkMagenta => new Pixel(SCEColor.DarkMagenta);
    public static Pixel DarkYellow => new Pixel(SCEColor.DarkYellow);
    public static Pixel Gray => new Pixel(SCEColor.Gray);
    public static Pixel DarkGray => new Pixel(SCEColor.DarkGray);
    public static Pixel Blue => new Pixel(SCEColor.Blue);
    public static Pixel Green => new Pixel(SCEColor.Green);
    public static Pixel Cyan => new Pixel(SCEColor.Cyan);
    public static Pixel Red => new Pixel(SCEColor.Red);
    public static Pixel Magenta => new Pixel(SCEColor.Magenta);
    public static Pixel Yellow => new Pixel(SCEColor.Yellow);
    public static Pixel White => new Pixel(SCEColor.White);

    #endregion

    public readonly Pixel Merge(Pixel other)
    {
        return new Pixel()
        {
            Element = Element.Merge(other.Element),
            FgColor = FgColor.Merge(other.FgColor),
            BgColor = BgColor.Merge(other.BgColor),
        };
    }

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
