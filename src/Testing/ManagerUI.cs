using SCENeo.Ui;

namespace SCENeo.Testing;

internal sealed class ManagerUi
{
    private readonly Updater _updater;

    private readonly Viewport _viewport;

    private readonly Display _display;

    private readonly TextLabel _textBox;

    private readonly ListBox _selector;

    public ManagerUi()
    {
        _updater = new Updater()
        {
            OnUpdate = Update,
            FrameCap = Updater.Uncapped,
        };

        _textBox = new TextLabel()
        {
            Width       = 30,
            Height      = 1,
            BasePixel   = new Pixel(SCEColor.Black, SCEColor.White),
            TextFgColor = SCEColor.Transparent,
            TextBgColor = SCEColor.Transparent,
            Visible     = false,
        };

        _selector = new ListBox()
        {
            Width     = 20,
            Height    = 16,
            BasePixel = Pixel.Null,
            Anchor    = Anchor.Right,
        };

        for (int i = 0; i < 16; i++)
        {
            SCEColor color = (SCEColor)i;

            var option = new ListBoxItem()
            {
                Text = color.ToString().PadRight(_selector.Width),
            };

            _selector.Items.Add(option);
        }


        _viewport = new Viewport()
        {
            BasePixel = new Pixel(SCEColor.DarkCyan),
            Source    = [_textBox, _selector],
        };

        _display = new Display()
        {
            Renderable = _viewport,
            Output = ConsoleOutput.Instance,
        };

        _display.Resized += Display_OnResize;
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

    private void Display_OnResize(int width, int height)
    {
        _viewport.Width  = width;
        _viewport.Height = height;
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
