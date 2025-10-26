using SCENeo.Node;
using SCENeo.UI;
using SCENeo.Utils;

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

        var rc1Filter = new Filter<RenderChannel>(rc1)
        {
            FilterMode = FilterPresets.BlackAndWhite,
        };

        var rc2 = new RenderChannel(30, 20)
        {
            BasePixel = new Pixel(SCEColor.DarkCyan),
            Anchor    = Anchor.Right | Anchor.Bottom,
        };

        var rc2Filter = new Filter<RenderChannel>(rc2)
        {
            FilterMode = FilterPresets.Grayscale, 
        };

        _fpsUI = new TextBoxUI(20, 2)
        {
            Text = "FPS: None",
        };

        _viewport.Renderables.AddEvery(rc1Filter, rc2, _fpsUI);

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
            Engines = [_re]
        };

        var cam1 = new Camera2D()
        {
            Name    = "Camera",
            Channel = 0,
        };

        var player = new Player()
        {
            Name     = "Player",
            Rotation = SCEUtils.DegToRad(15),
        };

        var block = new Sprite2D<DisplayMap>()
        {
            Name     = "Block",
            Position = new Vec2(30, 20),
            Source   = new DisplayMap(30, 5)
            {
                Anchor = Anchor.Right | Anchor.Bottom,
            },
        };

        var rand = new Random();

        block.Source.Fill(() => new Pixel((char)('a' + rand.Next(26)), SCEColor.White, (SCEColor)rand.Next(15)));

        _nm.Tree.Root.AddChildren(player, cam1, block);
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
