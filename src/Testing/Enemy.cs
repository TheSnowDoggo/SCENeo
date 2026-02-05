using SCENeo.Node.Boid;
using SCENeo.Node.Render;
using SCENeo.Ui;

namespace SCENeo.Testing;

internal sealed class Enemy : SteeredNode2D
{
    private readonly BoidPath _path;

    public Enemy()
    {
        var dpMap = new DisplayMap(2, 2)
        {
            Anchor = Anchor.Center | Anchor.Middle,
        };

        dpMap.Fill(new Pixel(SCEColor.DarkRed));

        var sprite = new Sprite2D()
        {
            Name = "Sprite2D",
            Source = new Stretcher()
            {
                Source     = dpMap,
                ScaleWidth = 2,
                Bake       = true,
            },
        };

        AddChild(sprite);

        _path = new BoidPath(this)
        {
            Points = [Vec2.Zero, new Vec2(30, 0), new Vec2(60, 40)],
            Mode   = BoidPath.Finish.Restart
        };
    }

    public override void Update(double delta)
    {
        SteeringManager.Seek(_path.CurrentTarget());

        Velocity = SteeringManager.NextVelocity();

        Move(delta);

        _path.Update();
    }
}
