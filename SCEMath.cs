namespace SCENeo;

public static class SCEMath
{
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
        return ((a % b) + b) % b;
    }

    /// <summary>
    /// Performs a linear interpolation.
    /// </summary>
    /// <param name="t">The ratio between min and max.</param>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    /// <returns>The interpolated result.</returns>
    public static float Lerp(this float t, float min, float max)
    {
        return float.Lerp(min, max, t);
    }

    /// <inheritdoc cref="Lerp(float,float,float)"/>
    public static double Lerp(this double t, double min, double max)
    {
        return double.Lerp(min, max, t);
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

    private const float FRadDegFactor = 180 / MathF.PI;
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
}