namespace SCENeo.Node.Boid;

internal abstract class SteeredNode2D : KinematicNode2D, IBoid
{
    protected readonly SteeringManager SteeringManager;

    public SteeredNode2D()
    {
        SteeringManager = new SteeringManager(this);
    }

    Vec2 IMove.Position { get { return GlobalPosition; } }

    Vec2 IMove.Velocity { get { return Velocity; } }

    public float MaxVelocity { get; set; } = 1f;

    public float Mass { get; set; } = 1f;

    protected override void Move(double delta)
    {
        Velocity = SteeringManager.NextVelocity();

        base.Move(delta);
    }
}
