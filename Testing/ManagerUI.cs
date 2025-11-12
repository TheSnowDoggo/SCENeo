using SCENeo.UI;
using SCENeo.Utils;

namespace SCENeo.Testing;

internal sealed class ManagerUI
{
    private readonly Updater _updater;

    private readonly Viewport _viewport;

    private readonly Display<Viewport> _display;

    private readonly TextBox _textBox;

    private readonly VerticalSelector _selector;

    private readonly ProgressBar _progressBar;

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

        _display = new Display<Viewport>(_viewport)
        {
            AutoResizing = true,
        };

        _textBox = new TextBox(30, 1)
        {
            BasePixel   = new Pixel(SCEColor.Black, SCEColor.White),
            TextFgColor = SCEColor.Transparent,
            TextBgColor = SCEColor.Transparent,
        };

        _selector = new VerticalSelector(20, 16)
        {
            BasePixel = Pixel.Transparent,
            Anchor    = Anchor.Right,
        };

        _progressBar = new ProgressBar(50, 1)
        {
            Max    = 100,
            Mode   = ProgressBar.FlowMode.LeftToRight,
            Offset = new Vec2I(0, 1),
        };

        for (int i = 0; i < 16; i++)
        {
            SCEColor color = (SCEColor)i;

            _selector[i] = new Option()
            {
                Text = color.ToString().PadRight(_selector.Width),
            };
        }

        _viewport.Renderables.AddEvery(_textBox, _selector, _progressBar);
    }

    public void Run()
    {
        _updater.Start();
    }

    private void Update(double delta)
    {
        _textBox.Text = $"FPS: {_updater.FPS}";

        if (Console.KeyAvailable)
        {
            OnInput(Console.ReadKey(true));
        }

        _progressBar.Value += delta * 30;

        _display.Update();
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
