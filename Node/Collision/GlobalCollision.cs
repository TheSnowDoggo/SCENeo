using SCENeo.Utils;

namespace SCENeo.Node.Collision;

internal static class GlobalCollision
{
    public static bool Collides(BoxCollider2D box1, BoxCollider2D box2)
    {
        return box1.GlobalArea().Overlaps(box2.GlobalArea());
    }

    public static bool Collides(SphereCollider2D sphere1, SphereCollider2D sphere2)
    {
        return sphere1.GlobalPosition.DistanceTo(sphere2.GlobalPosition) < sphere1.Radius + sphere2.Radius; 
    }

    public static bool Collides(SphereCollider2D sphere, BoxCollider2D box)
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
        Rect2D boxArea = box.GlobalArea();
        Vec2 rayEnd    = raycast.GlobalEnd();
        
        if (raycast.GlobalPosition.X == rayEnd.X)
        {
            return rayEnd.X >= boxArea.Left && rayEnd.X <= boxArea.Right;
        }

        float m = raycast.GlobalPosition.GradientBetween(rayEnd);
        float c = raycast.GlobalPosition.Y - m * raycast.GlobalPosition.X;

        SCEUtils.MinMax(raycast.GlobalPosition.Y, rayEnd.Y, out float minY, out float maxY);

        if (YIntersect(m, c, boxArea.Left).InInclusiveRange(minY, maxY) ||
            YIntersect(m, c, boxArea.Right).InInclusiveRange(minY, maxY))
        {
            return true;
        }

        SCEUtils.MinMax(raycast.GlobalPosition.X, rayEnd.X, out float minX, out float maxX);

        if (XIntersect(m, c, boxArea.Top   ).InInclusiveRange(minX, maxX) ||
            XIntersect(m, c, boxArea.Bottom).InInclusiveRange(minX, maxX))
        {
            return true;
        }

        return false;
    }
    
    private static float YIntersect(float m, float c, float x)
    {
        return m * x + c;
    }

    private static float XIntersect(float m, float c, float y)
    {
        return (y - c) / m;
    }
}
