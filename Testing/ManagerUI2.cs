using SCENeo.Node;
using SCENeo.Node.Render;
using SCENeo.Ui;

namespace SCENeo.Testing;

internal sealed class ManagerUI2
{
    private readonly Viewport _viewport;

    private readonly Display _display;

    private readonly TextBox _textBox;

    private readonly NodeManager _nodeManager;

    private readonly RenderEngine _renderEngine;

    private readonly RenderChannel _renderChannel;

    private readonly Updater _updater;

    public ManagerUI2()
    {
        _textBox = new TextBox()
        {
            Width  = 20,
            Height = 1,
            TextFgColor = SCEColor.White,
        };

        _renderChannel = new RenderChannel()
        {
            BasePixel = new Pixel(SCEColor.DarkCyan),
        };

        _viewport = new Viewport()
        {
            BasePixel = new Pixel(SCEColor.DarkGray),
            Source = [_textBox, _renderChannel],
        };

        _display = new Display()
        {
            Source = _viewport,
            Output = ConsoleOutput.Instance,
            OnResize = Display_OnResize,
        };

        _renderEngine = new RenderEngine()
        {
            Channels = new() { { 0, _renderChannel } }
        };

        _nodeManager = new NodeManager()
        {
            Engines = [_renderEngine],
        };

        var camera = new FreeCamera2D()
        {
            Name     = "camera",
            Position = new Vec2(-30, -10),
        };

        var ball = new Ball(2, 2)
        {
            Name     = "ball",
            Position = new Vec2(0, 0),
            Velocity = new Vec2(20, -30),
            Gravity  = new Vec2(0, 10),
        };

        ball.AddChild(camera);

        // can be very large as no grid is actually stored
        var floorVp = new VirtualPlane(int.MaxValue, int.MaxValue)
        {
            Value  = new Pixel(SCEColor.Green),
            Anchor = Anchor.Center | Anchor.Bottom,
        };

        var floor = new Sprite2D()
        {
            Name   = "floor",
            Source = floorVp,
        };

        _nodeManager.Tree.Root.AddChildren([ball, floor]);

        _updater = new Updater()
        {
            FrameCap = Updater.Uncapped,
            OnUpdate = Update,
        };
    }

    public void Run()
    {
        _nodeManager.Start();

        _updater.Start();
    }

    private void Update(double delta)
    {
        _textBox.Text = $"FPS: {_updater.FPS}";

        _nodeManager.Update(delta);

        _display.Update();
    }

    private void Display_OnResize(int width, int height)
    {
        _viewport.Width  = width;
        _viewport.Height = height;

        _renderChannel.Width  = width;
        _renderChannel.Height = height;
    }
}