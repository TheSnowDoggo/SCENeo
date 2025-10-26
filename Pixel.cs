using SCENeo.Utils;

namespace SCENeo;

public readonly struct Pixel(char element, ColorInfo colorInfo)
    : IEquatable<Pixel>
{
    public readonly char Element     = element;

    public readonly ColorInfo Colors = colorInfo;

    public Pixel(char element, SCEColor foregroundColor, SCEColor backgroundColor)
        : this(element, new ColorInfo(foregroundColor, backgroundColor))
    { }

    public Pixel(SCEColor backgroundColor)
        : this(' ', SCEColor.Black, backgroundColor)
    { }

    public static bool operator ==(Pixel p1, Pixel p2) => p1.Equals(p2);

    public static bool operator !=(Pixel p1, Pixel p2) => !p1.Equals(p2);

    public readonly Pixel Merge(Pixel other)
    {
        return new Pixel(Element.Merge(other.Element), Colors.Merge(other.Colors));
    }

    public readonly bool Equals(Pixel other)
    {
        return Element == other.Element && Colors == other.Colors;
    }

    public readonly override bool Equals(object? obj)
    {
        return obj is Pixel pixel && Equals(pixel);
    }

    public readonly override int GetHashCode()
    {
        return HashCode.Combine(Element, Colors.GetHashCode());
    }
}
