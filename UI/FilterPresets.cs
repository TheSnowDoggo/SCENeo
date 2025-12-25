namespace SCENeo.Ui;

public static class FilterPresets
{
    public static Pixel BlackAndWhite(Pixel pixel)
    {
        return pixel.BgColor.IsLight() ? new Pixel(pixel.Element, SCEColor.Black, SCEColor.White) 
            : new Pixel(pixel.Element, SCEColor.White, SCEColor.Black);
    }

    public static Pixel Grayscale(Pixel pixel)
    {
        return pixel.BgColor switch
        {
            SCEColor.Black       or
            SCEColor.Transparent
                => new Pixel(pixel.Element, SCEColor.White, SCEColor.Black),
            SCEColor.DarkBlue    or
            SCEColor.DarkGreen   or
            SCEColor.DarkRed     or
            SCEColor.DarkMagenta or
            SCEColor.DarkGray
                => new Pixel(pixel.Element, SCEColor.Gray, SCEColor.DarkGray),
            SCEColor.DarkCyan   or
            SCEColor.DarkYellow or
            SCEColor.Gray       or
            SCEColor.Blue       or 
            SCEColor.Green      or
            SCEColor.Magenta
                => new Pixel(pixel.Element, SCEColor.DarkGray, SCEColor.Gray),
            SCEColor.Cyan       or
            SCEColor.Red        or
            SCEColor.Yellow     or
            SCEColor.White
                => new Pixel(pixel.Element, SCEColor.Black, SCEColor.White),
            _ => throw new NotImplementedException("Unknown background color.")
        };
    }
}