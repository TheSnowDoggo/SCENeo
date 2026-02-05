using SCENeo.Node;
using SCENeo.Node.Render;
using SCENeo.Ui;

namespace SCENeo.Testing;

internal sealed class Ball : KinematicNode2D
{
    public Ball(int width, int height)
    {
        var sprite = new Sprite2D()
        {
            Source = new DisplayMap(Image.Plain(width * 2, height, new Pixel(SCEColor.Red))),
        };

        AddChild(sprite);
    }

    public Vec2 Gravity { get; set; } = new Vec2(0, 3.0f);

    public float FloorFriction { get; set; } = 10;

    public float Floor { get; set; } = 0;

    public override void Update(double delta)
    {
        Velocity += Gravity * (float)delta;

        Move(delta);

        Position = new Vec2(Position.X, Math.Min(Position.Y, Floor));

        if (Position.Y == 0)
        {
            Velocity -= new Vec2(Math.Sign(Velocity.X) * FloorFriction * (float)delta, 0);
        }
    }
}