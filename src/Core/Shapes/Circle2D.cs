namespace SCENeo;

public struct Circle2D
{
    public Vec2 Origin;
    public float Radius;

    public Circle2D(float radius)
    {
        Radius = radius;
    }

    public Circle2D(float radius, Vec2 origin)
        : this(radius)
    {
        Origin = origin;
    }

    public float Left => Origin.X - Radius;
    public float Top => Origin.Y + Radius;
    public float Right => Origin.X + Radius;
    public float Bottom => Origin.Y + Radius;

    public float GetY(float x)
    {
        float d = Radius.Squared() - (x - Origin.X).Squared();
        return d < 0 ? float.NaN : Origin.Y + MathF.Sqrt(d);
    }

    public float GetX(float y)
    {
        float d = Radius.Squared() - (y - Origin.Y).Squared();
        return d < 0 ? float.NaN : Origin.X + MathF.Sqrt(d);
    }

    public bool OverlapsVertical(float x, float top, float bottom)
    {
        float y = GetY(x);
        return !float.IsNaN(y) && y >= bottom && y <= top;
    }

    public bool OverlapsHorizontal(float y, float left, float right)
    {
        float x = GetX(y);
        return !float.IsNaN(x) && x >= left && x <= right;
    }

    public bool Overlaps(Rect2D area)
    {
        if (OverlapsVertical(area.Left, area.Top, area.Bottom))
        {
            return true;
        }

        if (OverlapsVertical(area.Right, area.Top, area.Bottom))
        {
            return true;
        }

        if (OverlapsHorizontal(area.Top, area.Left, area.Right))
        {
            return true;
        }

        if (OverlapsHorizontal(area.Bottom, area.Left, area.Right))
        {
            return true;
        }

        return false;
    }

    public bool Overlaps(Circle2D other)
    {
        return Origin.DistanceTo(other.Origin) <= Radius + other.Radius;
    }
}