namespace SCENeo;

public sealed class ConsoleOutput : IOutputSource
{
    private static Lazy<ConsoleOutput> Lazy = new(() => new ConsoleOutput());

    private readonly Grid2D<Pixel> _buffer = [];

    private ConsoleOutput() { }

    public static ConsoleOutput Instance
    {
        get {  return Lazy.Value; }
    }

    public int Width { get { return Console.WindowWidth; } }
    public int Height { get { return Console.WindowHeight; } }

    public void Update(IView<Pixel> view)
    {
        if (Width != _buffer.Width || Height != _buffer.Height)
        {
            _buffer.Resize(Width, Height);
        }

        int width  = Math.Min(_buffer.Width , view.Width );
        int height = Math.Min(_buffer.Height, view.Height);

        SCEColor lastFgColor = (SCEColor)Console.ForegroundColor;
        SCEColor lastBgColor = (SCEColor)Console.BackgroundColor;

        int curX = 0;
        int curY = 0;

        Console.SetCursorPosition(0, 0);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Pixel pixel = view[x, y];

                if (_buffer[x, y].LooksLike(pixel))
                {
                    continue;
                }

                pixel.Deconstruct(out char element, out SCEColor fgColor, out SCEColor bgColor);

                if (char.IsControl(element))
                {
                    element = ' ';
                }

                if (fgColor != lastFgColor)
                {
                    Console.ForegroundColor = fgColor.ToConsoleColor();
                    lastFgColor = fgColor;
                }

                if (bgColor != lastBgColor)
                {
                    Console.BackgroundColor = bgColor.ToConsoleColor();
                    lastBgColor = bgColor;
                }
                
                if (curX != x || curY != y)
                {
                    Console.SetCursorPosition(x, y);

                    (curX, curY) = Console.GetCursorPosition();
                }

                Console.Write(element);

                _buffer[x, y] = pixel;
            }
        }
    }
}
