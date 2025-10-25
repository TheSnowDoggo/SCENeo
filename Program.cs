using SCENeo.UI;
using SCENeo.Utils;

namespace SCENeo;

internal static class Program
{
    private static void Main()
    {
        Console.CursorVisible = false;

        var viewport = new Viewport(Console.WindowWidth, Console.WindowHeight)
        {
            BaseColor = new Pixel(SCEColor.DarkCyan),
        };

        var textBox = new TextBoxUI(20, 4)
        {
            Anchor       = Anchor.Center | Anchor.Middle,
            BasePixel    = new Pixel(SCEColor.Transparent),
            Text         = $"hello what the heck iss happening to this gaming",
            TextOverflow = true,
        };

        viewport.Renderables.Add(textBox);

        BufferUtils.WriteGrid(viewport.Render());

        Console.ReadLine();
    }
}