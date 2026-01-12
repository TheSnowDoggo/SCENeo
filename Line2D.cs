namespace SCENeo;

public struct Line2D
{
    public float Gradient;
    public float Intercept;

    public Line2D(float gradient, float intercept) 
    {
        Gradient = gradient;
        Intercept = intercept;
    }

    public static Line2D FromPoints(Vec2 a, Vec2 b)
    {
        if (a.X == b.X)
        {
            return new Line2D(float.NaN, a.X);
        }

        float m = (b.Y - a.Y) / (b.X - a.X);

        return new Line2D(m, a.Y - m * a.X);
    }

    public bool IsLineValid()
    {
        return float.IsNaN(Gradient);
    }

    public float GetY(float x)
    {
        return Gradient * x + Intercept;
    }

    public float GetX(float y)
    {
        return Gradient != 0 ? (y - Intercept) / Gradient : float.NaN;
    }

    public bool OverlapsVertical(float x, float top, float bottom)
    {
        float y = GetY(x);
        return y >= top && y <= bottom;
    }

    public bool OverlapsHorizontal(float y, float left, float top)
    {
        float x = GetX(y);
        return !float.IsNaN(x) && x >= left && x <= top;
    }

    public bool Overlaps(Rect2D area)
    {
        // Horizontal line
        if (Gradient == 0)
        {
            return Intercept >= area.Bottom && Intercept <= area.Top;
        }

        // Vertical line
        if (float.IsNaN(Gradient))
        {
            return Intercept >= area.Left && Intercept <= area.Right;
        }

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

    public bool Overlaps(Circle2D circle)
    {
        // Horizontal line
        if (Gradient == 0)
        {
            return Intercept >= circle.Bottom && Intercept <= circle.Top;
        }

        // Vertical line
        if (float.IsNaN(Gradient))
        {
            return Intercept >= circle.Left && Intercept <= circle.Right;
        }

        Intersect(circle, out float a, out float b, out float c);

        // No real roots
        if (SCEMath.Discriminant(a, b, c) < 0)
        {
            return false;
        }

        return true;
    }

    private void Intersect(Circle2D circle, out float a, out float b, out float c)
    {
        // equation for x-intersect: x^2(1 + m^2) + 2x(m(c - b) - a) + ((c - b)^2 + a^2 - r^2)

        float yChange = Intercept - circle.Origin.Y;

        a = Gradient * Gradient + 1;
        b = 2 * (Gradient * yChange - circle.Origin.X);
        c = yChange * yChange + circle.Origin.X * circle.Origin.X - circle.Radius * circle.Radius;
    }
}
