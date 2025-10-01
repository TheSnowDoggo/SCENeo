using System.Diagnostics.CodeAnalysis;

namespace SCENeo;

public struct Pixel(char element, ColorInfo colorInfo)
    : IEquatable<Pixel>
{
    public char Element     = element;

    public ColorInfo Colors = colorInfo;

    public Pixel(ConsoleColor backgroundColor)
        : this(' ', new ColorInfo(ConsoleColor.Black, backgroundColor))
    {
    }

    public static bool operator ==(Pixel p1, Pixel p2) => p1.Equals(p2);

    public static bool operator !=(Pixel p1, Pixel p2) => !p1.Equals(p2);

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
