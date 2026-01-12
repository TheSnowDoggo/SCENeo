namespace SCENeo.Node.Collision;

public abstract class Collider2D : Node2D, IReceiver, IListener
{
    public ushort Layers { get; set; }
    public ushort Masks { get; set; }
    public bool UseExactPosition { get; set; }

    public Action<IReceiver>? CollisionListen { get; set; }
    public Action<IListener>? CollisionReceive { get; set; }

    public abstract bool CollidesWith(IReceiver other);

    protected Vec2 GetPosition()
    {
        return UseExactPosition ? GlobalPosition : GlobalPosition.RoundedToPixel();
    }
}