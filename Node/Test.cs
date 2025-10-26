namespace SCENeo.Node;

internal sealed class Test : Node2D
{
    public double MoveSpeed = 5;

    public override void Update(double delta)
    {
        Position += new Vec2(1, 1).Normalized() * (float)(MoveSpeed * delta);
    }
}
