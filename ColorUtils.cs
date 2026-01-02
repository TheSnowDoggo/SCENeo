namespace SCENeo;

public static class ColorUtils
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
        return top == Pixel.ElementTransparent ? current : top;
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
}
