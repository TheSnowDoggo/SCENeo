namespace SCENeo.Utils;

internal static class AnchorUtils
{
    public static int AnchorHorizontal(this Anchor anchor, int difference)
    {
        bool right  = (anchor & Anchor.Right ) == Anchor.Right;
        bool center = (anchor & Anchor.Center) == Anchor.Center;

        if (!right && !center)
        {
            return 0;
        }

        if (!center)
        {
            return difference;
        }

        return right && int.IsOddInteger(difference) ? (difference / 2) + 1 : difference / 2;
    }

    public static int AnchorVertical(this Anchor anchor, int difference)
    {
        bool bottom = (anchor & Anchor.Bottom) == Anchor.Bottom;
        bool middle = (anchor & Anchor.Middle) == Anchor.Middle;

        if (!bottom && !middle)
        {
            return 0;
        }

        if (!middle)
        {
            return difference;
        }

        return bottom && int.IsOddInteger(difference) ? (difference / 2) + 1 : difference / 2;
    }

    public static Vec2I AnchorDimension(this Anchor anchor, Vec2I difference)
    {
        return new Vec2I(AnchorHorizontal(anchor, difference.X), AnchorVertical(anchor, difference.Y));
    }
}