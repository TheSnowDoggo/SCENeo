using SCENeo.UI;
using SCENeo.Node.Render;
using SCENeo.Node.Collision;
using SCENeo.Utils;

namespace SCENeo.Node;

internal sealed class Player : Node2D
{
    private Vec2 _moveVec;

    public double MoveSpeed = 5;

    public float Rotation = 0;

    public Player()
    {
        var camera = new Camera2D()
        {
            Name     = "Camera2D",
            Channel  = 1,
            Position = new Vec2(-8, -5),
        };

        var dpMap = new DisplayMap(4, 4)
        {
            Anchor = Anchor.Center | Anchor.Middle,
        };

        dpMap.Fill(new Pixel(SCEColor.White, SCEColor.DarkYellow));

        for (int y = 0; y < dpMap.Height; y++)
        {
            dpMap.MapLine("hello", 0, y, new ColorInfo(SCEColor.White, SCEColor.Transparent));
        }

        var sprite = new Sprite2D<HorizontalScaler<DisplayMap>>()
        {
            Name   = "Sprite2D",
            Source = new(dpMap, 2)
            {
                TextScaling = TextScaleMode.Slide,
                Bake        = true,
            },
        };

        var collider = new SphereCollider2D()
        {
            Radius = 2.0f,
            Layers = SCEUtils.CreateFlags(0),
            OnCollisionReceive = OnCollisionReceive,
        };
        
        AddChildren(camera, sprite, collider);
    }

    public override void Start()
    {
        _moveVec = Vec2.Right.Rotated(Rotation).Normalized();
    }

    public override void Update(double delta)
    {
        Position += _moveVec * (float)(MoveSpeed * delta);
    }

    private void OnCollisionReceive(IListen listener)
    {
        throw new Exception("collided");
    }
}
