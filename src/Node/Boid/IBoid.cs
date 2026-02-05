namespace SCENeo.Node.Boid;

internal interface IBoid : IMove
{
    float MaxVelocity { get; }
    float Mass { get; }
}