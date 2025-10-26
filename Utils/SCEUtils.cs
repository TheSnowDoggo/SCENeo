namespace SCENeo.Utils;

internal static class SCEUtils
{
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

    public static Vec2I WindowDimensions()
    {
        return new Vec2I(Console.WindowWidth, Console.WindowHeight);
    }

    public static void AddEvery<T>(this List<T> list, params T[] items)
    {
        list.AddRange(items);
    }

    #region CollisionData

    public static bool GetBit(this ushort data, int bit)
    {
        if (bit < 0 || bit > 15)
            throw new ArgumentOutOfRangeException(nameof(bit), bit, "Mask must be between 0 and 15.");
        return ((1 << data) & 1) == 1;
    }

    public static ushort SetBit(this ushort data, int bit, bool value)
    {
        if (bit < 0 || bit > 15)
            throw new ArgumentOutOfRangeException(nameof(bit), bit, "Mask must be between 0 and 15.");
        return (ushort)(value ? data | (1 << bit) : data & ~(1 << bit));
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
    public static float RadToDeg(float angle)
    {
        return angle * FRadDegFactor;
    }

    /// <summary>
    /// Converts from radians to degrees.
    /// </summary>
    /// <param name="angle">The angle in radians.</param>
    /// <returns>The resulting angle in degrees.</returns>
    public static double RadToDeg(double angle)
    {
        return angle * DRadDegFactor;
    }

    /// <summary>
    /// Converts from degrees to radians.
    /// </summary>
    /// <param name="angle">The angle in degrees.</param>
    /// <returns>The resulting angle in radians.</returns>
    public static float DegToRad(float angle)
    {
        return angle / FRadDegFactor;
    }

    /// <summary>
    /// Converts from degrees to radians.
    /// </summary>
    /// <param name="angle">The angle in degrees.</param>
    /// <returns>The resulting angle in radians.</returns>
    public static double DegToRad(double angle)
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
    public static float Lerp(float t, float min, float max)
    {
        return min + (max - min) * t;
    }

    /// <see cref="Lerp(float,float,float)"/>
    public static double Lerp(double t, double min, double max)
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
    public static float Unlerp(float value, float min, float max)
    {
        return (value - min) / (max - min);
    }

    /// <see cref="Unlerp(float,float,float)"/>
    public static double Unlerp(double value, double min, double max)
    {
        return (value - min) / (max - min);
    }

    #endregion
}