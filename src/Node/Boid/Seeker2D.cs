namespace SCENeo.Node.Boid;

internal class Seeker2D : SteeredNode2D
{
    public IMove Target { get; set; }

    public override void Update(double delta)
    {
        if (Target == null) return;

        SteeringManager.Pursuit(Target);

        Move(delta);
    }
}