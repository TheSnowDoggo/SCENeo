namespace SCENeo;

internal class Rect2DI
{
    public int Left;
    public int Top;
    public int Right;
    public int Bottom;

    public Rect2DI()
    {
    }

    public Rect2DI(int left, int top, int right, int bottom)
    {
        Left   = left;
        Top    = top;
        Right  = right;
        Bottom = bottom;
    }

    public bool Overlaps(int left, int top, int right, int bottom)
    {
        if (Right <= left || Left >= right) // X sides don't overlap
        {
            return false;
        }
        if (Bottom <= top || Top >= bottom) // Y sides don't overlap
        {
            return false;
        }
        return true;
    }

    public bool Overlaps(Rect2DI rect)
    {
        return Overlaps(rect.Left, rect.Top, rect.Right, rect.Bottom);
    }

    public void SetToOverlap(int left, int top, int right, int bottom)
    {
        Left   = Math.Max(Left  , left);
        Top    = Math.Max(Top   , top);
        Right  = Math.Min(Right , right);
        Bottom = Math.Min(Bottom, bottom);
    }

    public void SetToOverlap(Rect2DI rect)
    {
        SetToOverlap(rect.Left, rect.Top, rect.Right, rect.Bottom);
    }
}