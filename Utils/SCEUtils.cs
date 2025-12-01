namespace SCENeo.Utils;

public static class SCEUtils
{
    #region String

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

    #endregion

    #region UI

    public static void ObserveSet<T>(T value, ref T property, ref bool update)
    {
        if (!EqualityComparer<T>.Default.Equals(value, property))
        {
            property = value;
            update   = true;
        }
    }

    public static void Resize(this IResizeable resizeable, Vec2I dimensions)
    {
        resizeable.Resize(dimensions.X, dimensions.Y);
    }

    public static Vec2I ConsoleWindowSize()
    {
        return new Vec2I(Console.WindowWidth, Console.WindowHeight);
    }

    #endregion

    #region Collections

    public static void AddEvery<T>(this List<T> list, params T[] items)
    {
        list.AddRange(items);
    }

    #endregion

    #region Math

    public static void MinMax(float a, float b, out float min, out float max)
    {
        if (a < b)
        {
            min = a;
            max = b;
        }
        else
        {
            min = b;
            max = a;
        }
    }

    public static bool InFullRange(this float value, float min, float max)
    {
        return value >= min && value <= max;
    }

    public static float Squared(this float value)
    {
        return value * value;
    }

    public static int Mod(int a, int b)
    {
        int mod = a % b;
        return a >= 0 ? mod : mod + b;
    }

    #endregion

    #region Collision

    public static bool GetFlag(this ushort data, int flag)
    {
        if (flag < 0 || flag > 15)
            throw new ArgumentOutOfRangeException(nameof(flag), flag, "Mask must be between 0 and 15.");
        return ((1 << data) & 1) == 1;
    }

    public static ushort SetFlag(this ushort data, int flag, bool value)
    {
        if (flag < 0 || flag > 15)
            throw new ArgumentOutOfRangeException(nameof(flag), flag, "Mask must be between 0 and 15.");
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

    #endregion

    #region Grid2D

    public static void CleanResize<T>(this Grid2D<T> grid, Vec2I dimensions)
    {
        grid.CleanResize(dimensions.X, dimensions.Y);
    }

    #endregion

    #region IDimensioned

    public static int Size(this IDimensioned dimensioned)
    {
        return dimensioned.Width * dimensioned.Height;
    } 

    public static Vec2I Dimensions(this IDimensioned dimensioned)
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
        return dimensioned.InRange(pos.X, pos.Y);
    }

    #endregion

    #region Anchor

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

    #endregion

    #region Color

    public static ConsoleColor ToConsoleColor(this SCEColor color)
    {
        return color == SCEColor.Transparent ? ConsoleColor.Black : (ConsoleColor)color;
    }

    public static SCEColor Merge(this SCEColor current, SCEColor top)
    {
        return top == SCEColor.Transparent ? current : top;
    }

    public static char Merge(this char current, char top)
    {
        return top == '\0' ? current : top;
    }

    public static bool IsLight(this SCEColor color)
    {
        return color is SCEColor.White or SCEColor.Gray or SCEColor.Yellow or SCEColor.Cyan;
    }

    public static SCEColor Contrast(this SCEColor color)
    {
        return IsLight(color) ? SCEColor.Black : SCEColor.White;
    }

    public static SCEColor NextColor(this Random random, bool includeTransparent = false)
    {
        return includeTransparent ? (SCEColor)random.Next(16) : (SCEColor)random.Next(17) - 1;
    }

    #endregion

    #region Angle

    private const float  FRadDegFactor = 180 / MathF.PI;
    private const double DRadDegFactor = 180 / Math.PI;

    /// <summary>
    /// Converts from radians to degrees.
    /// </summary>
    /// <param name="angle">The angle in radians.</param>
    /// <returns>The resulting angle in degrees.</returns>
    public static float RadToDeg(this float angle)
    {
        return angle * FRadDegFactor;
    }

    /// <inheritdoc cref="RadToDeg(float)"/>
    public static double RadToDeg(this double angle)
    {
        return angle * DRadDegFactor;
    }

    /// <summary>
    /// Converts from degrees to radians.
    /// </summary>
    /// <param name="angle">The angle in degrees.</param>
    /// <returns>The resulting angle in radians.</returns>
    public static float DegToRad(this float angle)
    {
        return angle / FRadDegFactor;
    }

    /// <inheritdoc cref="DegToRad(float)"/>
    public static double DegToRad(this double angle)
    {
        return angle / DRadDegFactor;
    }

    #endregion

    #region Lerp

    /// <summary>
    /// Performs a linear interpolation.
    /// </summary>
    /// <param name="t">The ratio between min and max.</param>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    /// <returns>The interpolated result.</returns>
    public static float Lerp(this float t, float min, float max)
    {
        return min + (max - min) * t;
    }

    /// <inheritdoc cref="Lerp(float,float,float)"/>
    public static double Lerp(this double t, double min, double max)
    {
        return min + (max - min) * t;
    }

    /// <summary>
    /// Performs an inverse linear interpolation.
    /// </summary>
    /// <param name="value">The interpolated value.</param>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    /// <returns>The ratio between min and max of the value.</returns>
    public static float Unlerp(this float value, float min, float max)
    {
        return (value - min) / (max - min);
    }

    /// <inheritdoc cref="Unlerp(float,float,float)"/>
    public static double Unlerp(this double value, double min, double max)
    {
        return (value - min) / (max - min);
    }

    #endregion
}