using SCENeo.Node;
using SCENeo.UI;
using SCENeo.Utils;
using SCENeo.Node.Render;
using SCENeo.Node.Collision;

namespace SCENeo;

internal sealed class Manager
{
    private readonly Updater _updater;

    private readonly RenderEngine _re;

    private readonly NodeManager _nm;

    private readonly Viewport _viewport;

    private readonly Display<Viewport> _display;

    private readonly TextBoxUI _fpsUI;

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

        var rc1 = new RenderChannel()
        {
            BasePixel = new Pixel(SCEColor.DarkCyan),
        };

        var rc2 = new RenderChannel(40, 15)
        {
            BasePixel = new Pixel(SCEColor.Cyan),
            Anchor    = Anchor.Right | Anchor.Bottom,
        };

        var rc2Filter = new Filter<RenderChannel>(rc2)
        {
            FilterMode = null, 
        };

        _fpsUI = new TextBoxUI(20, 2)
        {
            Text   = "FPS: None",
            Anchor = Anchor.Right,
        };

        _viewport.Renderables.AddEvery(rc1, rc2Filter, _fpsUI);

        _re = new RenderEngine()
        {
            Channels = new() 
            {
                { 0, rc1 },
                { 1, rc2 },
            },
        };

        _nm = new NodeManager()
        {
            Engines = [_re, new CollisionEngine()],
        };

        var camera = new Camera2D()
        {
            Name    = "Camera",
            Channel = 0,
        };

        var player = new Player()
        {
            Name     = "Player",
            Rotation = SCEUtils.DegToRad(35.0f),
        };

        var box = new Box()
        {
            Name     = "Box",
            Position = new Vec2(30, 15),
        };

        _nm.Tree.Root.AddChildren(player, box, camera);
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

        _fpsUI.Text = $"FPS: {_updater.FPS}\n{_nm.Tree.Root.GetNode<Node2D>("Player/Sprite2D").GlobalPosition}";
    }

    private void Display_OnResize(Vec2I newDimensions)
    {
        _re.Channels[0].Resize(newDimensions);
    }
}
