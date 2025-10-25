namespace SCENeo.Utils;

internal static class AnchorUtils
{
    public static int AnchorHorizontal(this Anchor anchor, int source, int destination)
    {
        bool right  = (anchor & Anchor.Right ) == Anchor.Right;
        bool center = (anchor & Anchor.Center) == Anchor.Center;

        if (!right && !center)
        {
            return 0;
        }

        int difference = destination - source;

        if (!center)
        {
            return difference;
        }

        return right && difference.IsOdd() ? (difference / 2) + 1 : difference / 2;
    }

    public static int AnchorVertical(this Anchor anchor, int source, int destination)
    {
        bool bottom = (anchor & Anchor.Bottom) == Anchor.Bottom;
        bool middle = (anchor & Anchor.Middle) == Anchor.Middle;

        if (!bottom && !middle)
        {
            return 0;
        }

        int difference = destination - source;

        if (!middle)
        {
            return difference;
        }

        return bottom && difference.IsOdd() ? (difference / 2) + 1 : difference / 2;
    }

    public static Vec2I AnchorDimension(this Anchor anchor, Vec2I source, Vec2I destination)
    {
        return new Vec2I(AnchorHorizontal(anchor, source.X, destination.X), AnchorVertical(anchor, source.Y, destination.Y));
    }
}