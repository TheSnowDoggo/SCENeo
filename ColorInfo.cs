using SCENeo.Utils;

namespace SCENeo;

public struct ColorInfo(SCEColor foregroundColor, SCEColor backgroundColor)
    : IEquatable<ColorInfo>
{
    public SCEColor ForegroundColor = foregroundColor;
    public SCEColor BackgroundColor = backgroundColor;

    public static bool operator ==(ColorInfo c1, ColorInfo c2) => c1.Equals(c2);

    public static bool operator !=(ColorInfo c1, ColorInfo c2) => !c1.Equals(c2);

    public readonly ColorInfo Merge(ColorInfo other)
    {
        return new ColorInfo(ForegroundColor.Merge(other.ForegroundColor), BackgroundColor.Merge(other.BackgroundColor));
    }

    public readonly bool Equals(ColorInfo other)
    {
        return ForegroundColor == other.ForegroundColor &&
            BackgroundColor == other.BackgroundColor;
    }

    public readonly override bool Equals(object? other)
    {
        return other is ColorInfo colorInfo && Equals(colorInfo);
    }

    public readonly override int GetHashCode()
    {
        return HashCode.Combine(ForegroundColor, BackgroundColor);
    }

    public readonly override string ToString()
    {
        return $"{{ {ForegroundColor}, {BackgroundColor} }}";
    }
}