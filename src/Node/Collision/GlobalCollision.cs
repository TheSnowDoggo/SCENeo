namespace SCENeo.Node.Collision;

internal static class GlobalCollision
{
    public static bool Collides(BoxCollider2D box1, BoxCollider2D box2)
    {
        if (box1.Size <= Vec2.Zero || box2.Size <= Vec2.Zero)
        {
            return false;
        }

        return box1.GlobalArea().Overlaps(box2.GlobalArea());
    }

    public static bool Collides(CircleCollider2D circle1, CircleCollider2D circle2)
    {
        if (circle1.Radius <= 0 || circle2.Radius <= 0)
        {
            return false;
        }

        return circle1.GlobalCircle().Overlaps(circle2.GlobalCircle()); 
    }

    public static bool Collides(CircleCollider2D circle, BoxCollider2D box)
    {
        if (box.Size <= Vec2.Zero || circle.Radius <= 0)
        {
            return false;
        }

        return circle.GlobalCircle().Overlaps(box.GlobalArea());
    }

    public static bool Collides(RayCast2D raycast, BoxCollider2D box)
    {
        if (box.Size <= Vec2.Zero)
        {
            return false;
        }

        Rect2D boxArea = box.GlobalArea();

        if (raycast.Right < boxArea.Left || raycast.Left > boxArea.Right)
        {
            return false;
        }

        return raycast.GlobalLine().Overlaps(boxArea);
    }

    public static bool Collides(RayCast2D raycast, CircleCollider2D circle)
    {
        if (circle.Radius <= 0)
        {
            return false;
        }

        Circle2D circle2d = circle.GlobalCircle();

        if (raycast.Right < circle2d.Left || raycast.Left > circle2d.Right)
        {
            return false;
        }

        return raycast.GlobalLine().Overlaps(circle2d);
    }
}
