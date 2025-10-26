namespace SCENeo;

public class Image : Grid2D<Pixel>
{
    public Image(Pixel[,] data) : base(data) { }
    public Image() : base() { }
    public Image(int width, int height) : base(width, height) { }
    public Image(Vec2I dimensions) : base(dimensions) { }

    #region MergeMap

    public void MergeMap(Grid2DView<Pixel> view, Vec2I position, Rect2DI area)
    {
        Vec2I difference = area.Start - position;

        Rect2DI trim = area.Trim(difference, Dimensions + difference);

        Vec2I newPos = position + trim.Start;

        for (int y = trim.Top, curY = newPos.Y; y < trim.Bottom; y++, curY++)
        {
            for (int x = trim.Left, curX = newPos.X; x < trim.Right; x++, curX++)
            {
                this[curX, curY] = this[curX, curY].Merge(view[x, y]);
            }
        }
    }

    public void MergeMap(Grid2DView<Pixel> view, Rect2DI area)
    {
        MergeMap(view, Vec2I.Zero, area);
    }

    public void MergeMap(Grid2DView<Pixel> view, Vec2I position)
    {
        MergeMap(view, position, view.Area());
    }

    public void MergeMap(Grid2DView<Pixel> view)
    {
        MergeMap(view, Vec2I.Zero);
    }

    #endregion

    public bool MapLine(string line, int x, int y, ColorInfo colors)
    {
        if (y < 0 || y >= Height)
        {
            return false;
        }

        int startX = Math.Max(x, 0);

        if (startX >= Width)
        {
            return false;
        }

        int endX   = Math.Min(x + line.Length, Width);

        if (endX <= 0)
        {
            return false;
        }

        int i = startX - x;

        for (int curX = startX; curX < endX; curX++)
        {
            this[curX, y] = this[curX, y].Merge(new Pixel(line[i++], colors));
        }

        return true;
    }
}
