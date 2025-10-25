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

    public static bool IsEven(this int value)
    {
        return (value & 1) == 0;
    }

    public static bool IsOdd(this int value)
    {
        return (value & 1) == 1;
    }
}