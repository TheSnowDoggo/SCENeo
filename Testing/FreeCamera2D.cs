using SCENeo.Node.Render;

namespace SCENeo.Testing;

internal sealed class FreeCamera2D : Camera2D
{
    public double WalkSpeed { get; set; } = 5;

    public double SprintSpeed { get; set; } = 20;

    private bool _lastSingle = false;

    public override void Update(double delta)
    {
        Vec2 moveVec = MoveVector();

        if (moveVec == Vec2.Zero)
        {
            return;
        }

        bool single = moveVec.X == 0 || moveVec.Y == 0;

        // for smooth diaganol movement
        if (!single && _lastSingle)
        {
            Position = Position.Round();
        }

        _lastSingle = single;

        Position += moveVec.Normalized() * (float)(Speed() * delta);
    }

    private double Speed()
    {
        return WalkSpeed;
    }

    private static Vec2 MoveVector()
    {
        Vec2 moveVec = Vec2.Zero;

        return moveVec;
    }
}