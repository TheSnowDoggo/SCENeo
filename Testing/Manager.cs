using SCENeo.Node;
using SCENeo.Ui;
using SCENeo.Node.Render;
using SCENeo.Node.Collision;

namespace SCENeo.Testing;

internal sealed class Manager
{
    private readonly Updater _updater;

    private readonly RenderEngine _re;

    private readonly NodeManager _nm;

    private readonly Viewport _viewport;

    private readonly Display _display;

    private readonly TextBox _fpsUI;

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

        _display = new Display()
        {
            Source   = _viewport,
            Output   = ConsoleOutput.Instance,
            OnResize = Display_OnResize,
        };

        var rc1 = new RenderChannel()
        {
            BasePixel = new Pixel(SCEColor.DarkCyan),
        };

        var rc2 = new RenderChannel()
        {
            Width  = 40,
            Height = 15,
            BasePixel = new Pixel(SCEColor.Cyan),
            //Anchor    = Anchor.Right | Anchor.Bottom,
        };

        var rc2Filter = new Filter(rc2)
        {
            FilterMode = null, 
        };

        _fpsUI = new TextBox()
        {
            Width  = 20,
            Height = 2,
            Text   = "FPS: None",
            Anchor = Anchor.Right,
        };

        _viewport.Renderables.AddRange([rc1, rc2Filter, _fpsUI]);

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
            Rotation = SCEMath.DegToRad(10.0f),
        };

        var box = new Box()
        {
            Name     = "Box",
            Position = new Vec2(30, 15),
        };

        var enemy = new Enemy()
        {
            Name        = "Enemy",
            Position    = new Vec2(70, 70),
            MaxVelocity = 10,
        };

        _nm.Tree.Root.AddChildren(enemy, player, box, camera);
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

        _fpsUI.Text = $"FPS: {_updater.FPS}\n{_nm.Tree.Root.GetNode<Node2D>("Enemy").GlobalPosition}";
    }

    private void Display_OnResize(int width, int height)
    {
        _viewport.Width  = width;
        _viewport.Height = height;

        _re.Channels[1].Width = width;
        _re.Channels[1].Width = height;
    }
}
