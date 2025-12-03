using SCENeo.UI;
using SCENeo.Utils;

namespace SCENeo.Testing;

internal sealed class ManagerUI
{
    private readonly Updater _updater;

    private readonly Viewport _viewport;

    private readonly Display _display;

    private readonly TextBox _textBox;

    private readonly VerticalSelector _selector;

    public ManagerUI()
    {
        _updater = new Updater()
        {
            OnUpdate = Update,
            FrameCap = Updater.Uncapped,
        };

        _viewport = new Viewport()
        {
            BasePixel = new Pixel(SCEColor.DarkCyan),
        };

        _display = new Display()
        {
            Source   = _viewport,
            Output   = ConsoleOutput.Instance,
            OnResize = Display_OnResize,
        };

        _textBox = new TextBox(30, 1)
        {
            BasePixel   = new Pixel(SCEColor.Black, SCEColor.White),
            TextFgColor = SCEColor.Transparent,
            TextBgColor = SCEColor.Transparent,
            Enabled     = false,
        };

        _selector = new VerticalSelector(20, 16)
        {
            BasePixel = Pixel.Null,
            Anchor    = Anchor.Right,
        };

        for (int i = 0; i < 16; i++)
        {
            SCEColor color = (SCEColor)i;

            _selector[i] = new Option()
            {
                Text = color.ToString().PadRight(_selector.Width),
            };
        }

        _viewport.Renderables.AddEvery(_textBox, _selector);
    }

    public void Run()
    {
        _updater.Start();
    }

    private void Update(double delta)
    {
        _display.Update();

        OnInput(Console.ReadKey(true));
    }

    private void Display_OnResize(Vec2I newSize)
    {
        _viewport.Resize(newSize);
    }

    private void OnInput(ConsoleKeyInfo cki)
    {
        switch (cki.Key)
        {
        case ConsoleKey.UpArrow:
            _selector.WrapMove(-1);
            break;
        case ConsoleKey.DownArrow:
            _selector.WrapMove(+1);
            break;
        case ConsoleKey.Enter:
            _viewport.BasePixel = new Pixel((SCEColor)_selector.Selected);
            break;
        }
    }
}
