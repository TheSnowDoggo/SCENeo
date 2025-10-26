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
}