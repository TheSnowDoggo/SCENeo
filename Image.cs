using SCENeo.UI;
using SCENeo.Utils;

namespace SCENeo;

public class Image : Grid2D<Pixel>
{
    public Image(Pixel[,] data) : base(data) { }
    public Image(Grid2D<Pixel> data) : base(data) { }
    public Image() : base() { }
    public Image(int width, int height) : base(width, height) { }
    public Image(Vec2I dimensions) : base(dimensions) { }

    public static Image Plain(int width, int height, Pixel pixel)
    {
        var image = new Image(width, height);

        image.Fill(pixel);

        return image;
    }

    public static Image Plain(Vec2I dimensions, Pixel pixel)
    {
        return Plain(dimensions.X, dimensions.Y, pixel);
    }

    #region MergeMap

    public void MergeMap(IView<Pixel> view, Vec2I position, Rect2DI area)
    {
        Vec2I difference = area.Start - position;

        Rect2DI trim = area.Trim(difference, Dimensions + difference);

        position += trim.Start;

        for (int viewY = trim.Top, thisY = position.Y; viewY < trim.Bottom; viewY++, thisY++)
        {
            for (int viewX = trim.Left, thisX = position.X; viewX < trim.Right; viewX++, thisX++)
            {
                this[thisX, thisY] = this[thisX, thisY].Merge(view[viewX, viewY]);
            }
        }
    }

    public void MergeMap(Grid2DView<Pixel> view, Vec2I position, Rect2DI area)
    {
        MergeMap((IView<Pixel>)view, position, area);
    }

    public void MergeMap(IView<Pixel> view, Rect2DI area)
    {
        MergeMap(view, Vec2I.Zero, area);
    }

    public void MergeMap(Grid2DView<Pixel> view, Rect2DI area)
    {
        MergeMap((IView<Pixel>)view, area);
    }

    public void MergeMap(IView<Pixel> view, Vec2I position)
    {
        MergeMap(view, position, view.Area());
    }

    public void MergeMap(Grid2DView<Pixel> view, Vec2I position)
    {
        MergeMap((IView<Pixel>)view, position);
    }

    public void MergeMap(IView<Pixel> view)
    {
        MergeMap(view, Vec2I.Zero);
    }

    public void MergeMap(Grid2DView<Pixel> view)
    {
        MergeMap((IView<Pixel>)view);
    }

    #endregion

    public bool MapLine(string line, int x, int y, SCEColor fgColor, SCEColor bgColor)
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
            this[curX, y] = this[curX, y].Merge(new Pixel(line[i++], fgColor, bgColor));
        }

        return true;
    }

    public bool MapLine(string line, Vec2I pos, SCEColor fgColor, SCEColor bgColor)
    {
        return MapLine(line, pos.X, pos.Y, fgColor, bgColor);
    }
}
