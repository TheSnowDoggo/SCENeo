namespace SCENeo;

internal struct Pixel(char element, ColorInfo colorInfo)
{
    public char Element = element;

    public ColorInfo Colors = colorInfo;

    public Pixel(ConsoleColor backgroundColor)
        : this(' ', new ColorInfo(ConsoleColor.Black, backgroundColor))
    {
    }
}
