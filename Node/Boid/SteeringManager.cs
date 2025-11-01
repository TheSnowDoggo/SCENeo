using SCENeo.Utils;

namespace SCENeo.Node.Boid;

internal sealed class SteeringManager(IBoid host)
{
    private static readonly Random _random = new Random();

    private readonly IBoid _host = host;

    private Vec2 _steering = Vec2.Zero;

    public float WanderCircleDistance { get; set; } = 15f;
    public float WanderCircleRadius { get; set; } = 5f;

    /// <summary>
    /// Returns the hosts next velocity and resets the current steering.
    /// </summary>
    /// <remarks>
    /// Should only be called once per frame as steering is reset.
    /// </remarks>
    /// <returns>The hosts next velocity.</returns>
    public Vec2 NextVelocity(float maxForce = float.PositiveInfinity)
    {
        Vec2 velocity = _host.Velocity;

        Vec2 steering = _steering.Truncate(maxForce) / _host.Mass;

        Vec2 nextVelocity = (velocity + steering).Truncate(_host.MaxVelocity);

        _steering = Vec2.Zero;

        return nextVelocity;
    }

    /// <summary>
    /// Resets the current steering vector.
    /// </summary>
    public void Reset()
    {
        _steering = Vec2.Zero;
    }

    #region SteeringAPI

    public void Seek(Vec2 target, float slowingRadius = 0f)
    {
        _steering += GetSeek(target, slowingRadius);
    }

    public void Flee(Vec2 target)
    {
        _steering += GetFlee(target);
    }

    public void Wander(float maxAngle = 1f)
    {
        _steering += GetWander(maxAngle);
    }

    public void Pursuit(IMove target)
    {
        _steering += GetPursuit(target);
    }

    public void Evade(IMove target)
    {
        _steering += GetEvade(target);
    }

    #endregion

    #region Implementation

    public Vec2 GetFuturePosition(IMove target)
    {
        float distance = _host.Position.DistanceTo(target.Position);

        float time = distance / _host.MaxVelocity;

        Vec2 futurePosition = target.Position + target.Velocity * time;

        return futurePosition;
    }

    private Vec2 GetSeek(Vec2 target, float slowingRadius = 0f)
    {
        if (_host.Position == target)
        {
            return Vec2.Zero;
        }

        Vec2 desiredVelocity = target - _host.Position;

        float distance = desiredVelocity.Length();

        desiredVelocity = desiredVelocity.Normalized() * _host.MaxVelocity;

        if (distance < slowingRadius)
        {
            float ratio = distance / slowingRadius;

            desiredVelocity *= ratio;
        }

        Vec2 steering = desiredVelocity - _host.Velocity;

        return steering;
    }

    private Vec2 GetFlee(Vec2 target)
    {
        Vec2 desiredVelocity = (target.DirectionTo(_host.Position) * _host.MaxVelocity);

        Vec2 steering = desiredVelocity - _host.Velocity;

        return steering;
    }

    private Vec2 GetWander(float maxAngle)
    {
        Vec2 direction = _host.Velocity == Vec2.Zero ? Vec2.Right : _host.Velocity.Normalized();

        Vec2 circleCenter = direction * WanderCircleDistance;

        Vec2 displacement = direction * WanderCircleRadius;

        float angle = _random.NextSingle().Lerp(-maxAngle, maxAngle);

        displacement = displacement.Rotated(angle);

        return circleCenter + displacement;
    }

    private Vec2 GetPursuit(IMove target)
    {
        return GetSeek(GetFuturePosition(target));
    }

    private Vec2 GetEvade(IMove target)
    {
        return GetFlee(GetFuturePosition(target));
    }

    #endregion
}