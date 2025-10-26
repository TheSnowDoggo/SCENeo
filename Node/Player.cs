using SCENeo.UI;

namespace SCENeo.Node;

internal sealed class Player : Node2D
{
    private Vec2 _moveVec;

    public double MoveSpeed = 5;

    public float Rotation = 0;

    public Player()
    {
        var cam2 = new Camera2D()
        {
            Name     = "Camera2D",
            Channel  = 1,
            Position = new Vec2(-15, -10),
        };

        var sprite = new Sprite2D<DisplayMap>()
        {
            Name   = "Sprite2D",
            Source = new DisplayMap(10, 5)
            {
                Anchor = Anchor.Center | Anchor.Middle,
            },
        };

        sprite.Source.Fill(new Pixel(SCEColor.Yellow));

        AddChildren(cam2, sprite);
    }

    public override void Start()
    {
        _moveVec = Vec2.Right.Rotated(Rotation).Normalized();
    }

    public override void Update(double delta)
    {
        Position += _moveVec * (float)(MoveSpeed * delta);
    }
}
