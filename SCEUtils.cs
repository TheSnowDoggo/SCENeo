namespace SCENeo;

/// <summary>
/// A static class containing miscellaneous utilitiy functions. 
/// </summary>
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
        for (int i = 0; i < 16; i++)
        {
            if (((data >> i) & 1) == 1)
            {
                yield return i;
            }
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

    public static void Swap<T>(ref T a, ref T b)
    {
        (a, b) = (b, a);
    }

    public static IEnumerator<T> EnumerateData<T>(IView<T> view)
    {
        for (int y = 0; y < view.Height; y++)
        {
            for (int x = 0; x < view.Width; x++)
            {
                yield return view[x, y];
            }
        }
    }

    public static IEnumerable<Vec2I> EnumerateArea(int left, int top, int right, int bottom)
    {
        for (int y = top; y < bottom; y++)
        {
            for (int x = left; x < right; x++)
            {
                yield return new Vec2I(x, y);
            }
        }
    }

    public static IEnumerable<Vec2I> EnumerateArea(int width, int height)
    {
        return EnumerateArea(0, 0, width, height);
    }

    public static IEnumerable<Vec2I> EnumerateArea(Vec2I size)
    {
        return EnumerateArea(0, 0, size.X, size.Y);
    }

    public static IEnumerable<Vec2I> EnumerateArea<T>(IView<T> view)
    {
        return EnumerateArea(view.Width, view.Height);
    }
}