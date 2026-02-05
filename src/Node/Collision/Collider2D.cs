namespace SCENeo.Node.Collision;

public abstract class Collider2D : Node2D, IReceiver, IListener
{
    public ushort Layers { get; set; }
    public ushort Masks { get; set; }
    public PositionRounding Rounding { get; set; }

    public Action<IReceiver> Listen { get; set; }
    public Action<IListener> Receive { get; set; }

    public abstract bool CollidesWith(IReceiver other);

    protected Vec2 GetPosition()
    {
        return GlobalPosition.RoundAsRenderCoords(Rounding);
    }
}