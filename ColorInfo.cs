using SCENeo.Utils;

namespace SCENeo;

public struct ColorInfo(SCEColor fgColor, SCEColor bgColor)
    : IEquatable<ColorInfo>
{
    public SCEColor FgColor = fgColor;
    public SCEColor BgColor = bgColor;

    public static bool operator ==(ColorInfo c1, ColorInfo c2) => c1.Equals(c2);

    public static bool operator !=(ColorInfo c1, ColorInfo c2) => !c1.Equals(c2);

    public readonly ColorInfo Merge(ColorInfo other)
    {
        return new ColorInfo(FgColor.Merge(other.FgColor), BgColor.Merge(other.BgColor));
    }

    public void Deconstruct(out SCEColor foreground, out SCEColor background)
    {
        foreground = FgColor;
        background = BgColor;
    }

    public readonly bool Equals(ColorInfo other)
    {
        return FgColor == other.FgColor &&
            BgColor == other.BgColor;
    }

    public readonly override bool Equals(object? other)
    {
        return other is ColorInfo colorInfo && Equals(colorInfo);
    }

    public readonly override int GetHashCode()
    {
        return HashCode.Combine(FgColor, BgColor);
    }

    public readonly override string ToString()
    {
        return $"{{ {FgColor}, {BgColor} }}";
    }
}