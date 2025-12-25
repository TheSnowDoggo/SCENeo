namespace SCENeo.Node.Collision;

internal static class GlobalCollision
{
    public static bool Collides(BoxCollider2D box1, BoxCollider2D box2)
    {
        return box1.GlobalArea().Overlaps(box2.GlobalArea());
    }

    public static bool Collides(CircleCollider2D circle1, CircleCollider2D circle2)
    {
        return circle1.GlobalPosition.DistanceTo(circle2.GlobalPosition) <= circle1.Radius + circle2.Radius; 
    }

    public static bool Collides(CircleCollider2D sphere, BoxCollider2D box)
    {
        Rect2D sphereArea = sphere.GlobalArea();
        Rect2D boxArea    = box.GlobalArea();

        if (!sphereArea.Overlaps(boxArea))
        {
            return false;
        }

        if (boxArea.Contains(sphereArea) || sphereArea.Contains(boxArea))
        {
            return true;
        }

        if (sphere.OverlapsVertical  (boxArea.Left  , boxArea.Top , boxArea.Bottom) ||
            sphere.OverlapsVertical  (boxArea.Right , boxArea.Top , boxArea.Bottom) ||
            sphere.OverlapsHorizontal(boxArea.Top   , boxArea.Left, boxArea.Right ) ||
            sphere.OverlapsHorizontal(boxArea.Bottom, boxArea.Left, boxArea.Right ))
        {
            return true;
        }

        return false;
    }

    public static bool Collides(RayCast2D raycast, BoxCollider2D box)
    {
        Vec2 start = raycast.GlobalPosition;
        Vec2 end   = raycast.GlobalEnd();

        Rect2D lineArea = start.AreaBetween(end);
        Rect2D boxArea  = box.GlobalArea();

        if (!boxArea.Overlaps(lineArea))
        {
            return false;
        }

        if (lineArea.Left == lineArea.Right)
        {
            return true;
        }

        if (boxArea.Contains(lineArea))
        {
            return true;
        }

        start.LineBetween(end, out float m, out float c);

        if (YIntersectVertical  (m, c, boxArea.Left  ).InFullRange(boxArea.Top , boxArea.Bottom) ||
            YIntersectVertical  (m, c, boxArea.Right ).InFullRange(boxArea.Top , boxArea.Bottom) ||
            XIntersectHorizontal(m, c, boxArea.Top   ).InFullRange(boxArea.Left, boxArea.Right ) ||
            XIntersectHorizontal(m, c, boxArea.Bottom).InFullRange(boxArea.Left, boxArea.Right ))
        {
            return true;
        }

        return false;
    }

    public static bool Collides(RayCast2D raycast, CircleCollider2D circle)
    {
        raycast.GlobalPosition.LineBetween(raycast.GlobalEnd(), out float l_m, out float l_c);

        circle.GlobalPosition.Deconstruct(out float c_a, out float c_b);

        float c_r = circle.Radius;

        float q_a = l_m.Squared() + 1;
        float q_b = (l_m * (l_c - c_b) - c_a) * 2;
        float q_c = (l_c - c_b).Squared() + c_a.Squared() - c_r.Squared();

        float discriminant = q_b.Squared() - (4 * q_a * q_c);

        return discriminant >= 0;
    }
    
    private static float YIntersectVertical(float m, float c, float x)
    {
        return m * x + c;
    }

    private static float XIntersectHorizontal(float m, float c, float y)
    {
        return (y - c) / m;
    }
}
