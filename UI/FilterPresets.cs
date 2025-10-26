using SCENeo.Utils;

namespace SCENeo.UI;

public static class FilterPresets
{
    public static Pixel BlackWhite(Pixel pixel)
    {
        return pixel.Colors.BackgroundColor.IsLight() ? new Pixel(SCEColor.White) : new Pixel(SCEColor.Black);
    }

    public static Pixel Grayscale(Pixel pixel)
    {
        return pixel.Colors.BackgroundColor switch
        {
            SCEColor.Black       or
            SCEColor.Transparent
                => new Pixel(SCEColor.Black),
            SCEColor.DarkBlue    or
            SCEColor.DarkGreen   or
            SCEColor.DarkRed     or
            SCEColor.DarkMagenta or
            SCEColor.DarkGray
                => new Pixel(SCEColor.DarkGray),
            SCEColor.DarkCyan   or
            SCEColor.DarkYellow or
            SCEColor.Gray       or
            SCEColor.Blue       or 
            SCEColor.Green      or
            SCEColor.Magenta
                => new Pixel(SCEColor.Gray),
            SCEColor.Cyan       or
            SCEColor.Red        or
            SCEColor.Yellow     or
            SCEColor.White
                => new Pixel(SCEColor.White),
            _ => throw new NotImplementedException("Unknown background color.")
        };
    }
}