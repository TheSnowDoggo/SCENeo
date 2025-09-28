namespace SCENeo;

internal static class Program
{
    private static readonly Image _buffer = new(Console.WindowWidth, Console.WindowHeight);

    private static readonly Updater _updater = new()
    {
        Update = Update,
    };

    private static void Main()
    {
        Console.CursorVisible = false;

        _updater.FrameCap = 60;
        _updater.Start();

        Console.ReadLine();

        _updater.Stop();
    }

    private static void Update(double delta)
    {
        _buffer.MapString(string.Join('\n', 
            $"FPS        {_updater.FPS}",
            $"Delta      {_updater.Delta}s",
            $"Real Delta {_updater.RealDelta}s"), 
            new ColorInfo(ConsoleColor.White, ConsoleColor.Black), 0, 0);

        BufferUtils.WriteGrid(_buffer);

        _buffer.Clear();
    }
}