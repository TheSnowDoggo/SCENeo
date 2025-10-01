namespace SCENeo;

public struct ColorInfo(ConsoleColor foregroundColor, ConsoleColor backgroundColor)
    : IEquatable<ColorInfo>
{
    public ConsoleColor ForegroundColor = foregroundColor;
    public ConsoleColor BackgroundColor = backgroundColor;

    public static bool operator ==(ColorInfo c1, ColorInfo c2) => c1.Equals(c2);

    public static bool operator !=(ColorInfo c1, ColorInfo c2) => !c1.Equals(c2);

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

    public override string ToString()
    {
        return $"{{ {ForegroundColor}, {BackgroundColor} }}";
    }
}