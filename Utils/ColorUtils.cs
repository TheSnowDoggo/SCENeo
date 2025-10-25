namespace SCENeo.Utils;

internal static class ColorUtils
{
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
}