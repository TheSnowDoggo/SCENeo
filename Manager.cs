using SCENeo.Node;
using SCENeo.UI;
using SCENeo.Utils;

namespace SCENeo;

internal sealed class Manager
{
    private readonly Updater _updater;

    private readonly NodeManager _nm;

    private readonly Viewport _viewport;

    private readonly Display<Viewport> _display;

    private readonly TextBoxUI _fpsUI;

    private readonly RenderChannel _renderChannel;

    public Manager()
    {
        _updater = new Updater()
        {
            OnUpdate = Update,
        };

        _viewport = new Viewport()
        {
            BasePixel = new Pixel(SCEColor.DarkGray),
        };

        _display = new Display<Viewport>(_viewport)
        {
            AutoResizing = true,
            OnResize     = Display_OnResize,
        };

        _renderChannel = new RenderChannel()
        {
            BasePixel = new Pixel(SCEColor.DarkCyan),
        };

        _fpsUI = new TextBoxUI(20, 2)
        {
            Text = "FPS: None",
        };

        _viewport.Renderables.AddEvery(_renderChannel, _fpsUI);

        _nm = new NodeManager();

        _nm.Channels[0] = _renderChannel;

        var camera = new Camera2D()
        {
            Name    = "Camera",
            Channel = 0,
        };

        _nm.Tree.Root.AddChild(camera);

        var sprite = new Sprite2D<DisplayMap>()
        {
            Name   = "Sprite2D",
            Source = new DisplayMap(10, 5)
            {
                Anchor = Anchor.Right | Anchor.Bottom,
            },
        };

        sprite.Source.Fill(new Pixel(SCEColor.Yellow));

        var test = new Test()
        {
            Name = "Test",
        };

        test.AddChild(sprite);

        _nm.Tree.Root.AddChild(test);
    }

    public void Run()
    {
        Start();
        _updater.Start();
    }

    private void Start()
    {
        _nm.Start();
    }

    private void Update(double delta)
    {
        _nm.Update(delta);

        _display.Update();

        _fpsUI.Text = $"FPS: {_updater.FPS}\n{_nm.Tree.Root.GetNode<Node2D>("Test/Sprite2D").GlobalPosition}";
    }

    private void Display_OnResize(Vec2I newDimensions)
    {
        _renderChannel.Resize(newDimensions);
    }
}
