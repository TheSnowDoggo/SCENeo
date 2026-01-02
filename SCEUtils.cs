namespace SCENeo;

public static class SCEUtils
{
    public static int IndexOf(this string str, Predicate<char> predicate, int start)
    {
        if (start < 0)
        {
            throw new ArgumentException("Start index must not be negative.");
        }

        for (int i = start; i < str.Length; i++)
        {
            if (predicate.Invoke(str[i]))
            {
                return i;
            }
        }

        return -1;
    }

    public static int IndexOf(this string str, Predicate<char> predicate)
    {
        return IndexOf(str, predicate, 0);
    }

    public static void ObserveSet<T>(T value, ref T property, ref bool update)
    {
        if (!EqualityComparer<T>.Default.Equals(value, property))
        {
            property = value;
            update   = true;
        }
    }

    public static bool GetFlag(this ushort data, int flag)
    {
        if (flag < 0 || flag > 15)
        {
            throw new ArgumentOutOfRangeException(nameof(flag), flag, "Mask must be between 0 and 15.");
        }

        return ((1 << data) & 1) == 1;
    }

    public static ushort SetFlag(this ushort data, int flag, bool value)
    {
        if (flag < 0 || flag > 15)
        {
            throw new ArgumentOutOfRangeException(nameof(flag), flag, "Mask must be between 0 and 15.");
        }

        return (ushort)(value ? data | (1 << flag) : data & ~(1 << flag));
    }

    public static IEnumerable<int> EnumerateFlags(this ushort data)
    {
        ushort cur = data;
        for (int i = 0; i < 16; i++)
        {
            if ((cur & 1) == 1)
            {
                yield return i;
            }

            cur <<= 1;
        }
    }

    public static ushort CreateFlags(params int[] flags)
    {
        ushort data = 0;
        foreach (int bit in flags)
        {
            data = SetFlag(data, bit, true);
        }
        return data;
    }

    public static void CleanResize<T>(this Grid2D<T> grid, Vec2I dimensions)
    {
        grid.CleanResize(dimensions.X, dimensions.Y);
    }

    public static Vec2I Size(this IDimensioned dimensioned)
    {
        return new Vec2I(dimensioned.Width, dimensioned.Height);
    }

    public static Rect2DI Area(this IDimensioned dimensioned)
    {
        return new Rect2DI(dimensioned.Width, dimensioned.Height);
    }

    public static bool InRange(this IDimensioned dimensioned, int x, int y)
    {
        return x >= 0 && y >= 0 && x < dimensioned.Width && y < dimensioned.Height;
    }

    public static bool InRange(this IDimensioned dimensioned, Vec2I pos)
    {
        return pos.X >= 0 && pos.Y >= 0 && pos.X < dimensioned.Width && pos.Y < dimensioned.Height;
    }

    public static int AnchorHorizontal(this Anchor anchor, int difference)
    {
        bool right = (anchor & Anchor.Right) == Anchor.Right;
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