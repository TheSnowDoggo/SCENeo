namespace SCENeo.Node.Boid;

internal sealed class BoidPath(IMove host)
{
    public enum Finish
    {
        None,
        Restart,
        Turn,
    }

    private readonly IMove _host = host;

    public List<Vec2> Points { get; init; } = [];

    public float PointCompletionRadius { get; set; } = 1f;

    public bool Reverse { get; set; } = false;

    public Finish Mode { get; set; } = Finish.None;

    public int Current { get; set; } = 0;

    public bool IsFinished { get { return !InRange(); } }

    public Vec2 CurrentTarget()
    {
        return InRange() ? Points[Current] : _host.Position;
    }

    public void Update()
    {
        if (!InRange())
        {
            if (Mode == Finish.None)
            {
                return;
            }

            if (Mode == Finish.Turn)
            {
                Reverse = !Reverse;
            }

            Restart();

            return;
        }

        if (_host.Position.DistanceTo(Points[Current]) <= PointCompletionRadius)
        {
            Current += Reverse ? -1 : +1;
        }
    }

    public void Restart()
    {
        Current = Reverse ? Points.Count - 1 : 0;
    }

    private bool InRange()
    {
        return Current >= 0 && Current < Points.Count;
    }
}