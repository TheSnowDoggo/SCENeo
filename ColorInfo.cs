namespace SCENeo;

internal struct ColorInfo(ConsoleColor foregroundColor, ConsoleColor backgroundColor)
{
    public ConsoleColor ForegroundColor = foregroundColor;
    public ConsoleColor BackgroundColor = backgroundColor;
}