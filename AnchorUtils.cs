namespace SCENeo;

public static class AnchorUtils
{
    private enum Horizontal
    {
        Left,
        Right,
        CenterLeft,
        CenterRight,
    }

    private enum Vertical
    {
        Top,
        Bottom,
        MiddleTop,
        MiddleBottom,
    }

    public static int AnchorHorizontal(this Anchor anchor, int difference)
    {
        return anchor.GetHorizontal() switch
        {
            Horizontal.Left        => 0,
            Horizontal.Right       => difference,
            Horizontal.CenterLeft  => difference / 2,
            Horizontal.CenterRight => int.IsOddInteger(difference) ? (difference / 2) + 1 : difference / 2,
            _ => throw new Exception($"Unknown horizontal state")
        };
    }

    public static int AnchorVertical(this Anchor anchor, int difference)
    {
        return anchor.GetVertical() switch
        {
            Vertical.Top          => 0,
            Vertical.Bottom       => difference,
            Vertical.MiddleTop    => difference / 2,
            Vertical.MiddleBottom => int.IsOddInteger(difference) ? (difference / 2) + 1 : difference / 2,
            _ => throw new Exception($"Unknown horizontal state")
        };
    }

    public static Vec2I AnchorDimension(this Anchor anchor, Vec2I difference)
    {
        return new Vec2I(AnchorHorizontal(anchor, difference.X), AnchorVertical(anchor, difference.Y));
    }

    public static float AnchorHorizontal(this Anchor anchor, float difference)
    {
        if (anchor.Contains(Anchor.Center))
        {
            return difference / 2;
        }

        if (anchor.Contains(Anchor.Right))
        {
            return difference;
        }

        return 0;
    }

    public static float AnchorVertical(this Anchor anchor, float difference)
    {
        if (anchor.Contains(Anchor.Middle))
        {
            return difference / 2;
        }

        if (anchor.Contains(Anchor.Bottom))
        {
            return difference;
        }

        return 0;
    }

    public static Vec2 AnchorDimension(this Anchor anchor, Vec2 difference)
    {
        return new Vec2(AnchorHorizontal(anchor, difference.X), AnchorVertical(anchor, difference.Y));
    }

    public static string FitToLength(this string text, int length, char fill, Anchor anchor = Anchor.None)
    {
        if (text.Length == length)
        {
            return text;
        }

        bool more = text.Length > length;

        switch (anchor.GetHorizontal())
        {
        case Horizontal.Left:
            return more ? text[..length] : text.PadRight(length, fill);
        case Horizontal.Right:
            return more ? text[(text.Length - length)..] : text.PadLeft(length, fill);
        case Horizontal.CenterLeft:
            if (more)
            {
                return text.Substring((text.Length - length) / 2, length);
            }
            else
            {
                int difference = length - text.Length;
                int half = difference / 2;

                return new string(fill, half) + text + new string(fill, difference - half);
            }
        case Horizontal.CenterRight:
            if (more)
            {
                int difference = text.Length - length;
                int half = int.IsEvenInteger(text.Length) ? difference / 2 : (difference / 2) + 1;

                return text.Substring(half, length);
            }
            else
            {
                int difference = length - text.Length;
                int half = int.IsEvenInteger(text.Length) ? difference / 2 : (difference / 2) + 1;

                return new string(fill, half) + text + new string(fill, difference - half);
            }
        default:
            throw new Exception($"Unknown horizontal state");
        }
    }

    public static string FitToLength(this string text, int length, Anchor anchor = Anchor.None)
    {
        return FitToLength(text, length, ' ', anchor);
    }

    public static bool Contains(this Anchor anchor, Anchor value)
    {
        return (anchor & value) == value;
    }

    private static Horizontal GetHorizontal(this Anchor anchor)
    {
        bool right = anchor.Contains(Anchor.Right);

        if (anchor.Contains(Anchor.Center))
        {
            return right ? Horizontal.CenterRight : Horizontal.CenterLeft;
        }

        return right ? Horizontal.Right : Horizontal.Left;
    }

    private static Vertical GetVertical(this Anchor anchor)
    {
        bool bottom = anchor.Contains(Anchor.Bottom);

        if (anchor.Contains(Anchor.Middle))
        {
            return bottom ? Vertical.MiddleBottom : Vertical.MiddleTop;
        }

        return bottom ? Vertical.Bottom : Vertical.Top;
    }
}
