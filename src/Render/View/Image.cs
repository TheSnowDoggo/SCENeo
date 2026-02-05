namespace SCENeo;

/// <summary>
/// Represents a specialized <see cref="Pixel"/> grid with useful functions.
/// </summary>
public class Image : Grid2D<Pixel>
{
    public Image()
        : base()
    {
    }

    public Image(Pixel[,] data) 
        : base(data) 
    { 
    }

    public Image(Grid2D<Pixel> data) 
        : base(data)
    { 
    }

    public Image(int width, int height) 
        : base(width, height) 
    { 
    }

    public Image(Vec2I size) 
        : base(size) 
    { 
    }

    /// <summary>
    /// Creates an image filled with the given pixel.
    /// </summary>
    /// <param name="width">The width of the image.</param>
    /// <param name="height">The height of the image.</param>
    /// <param name="pixel">The pixel to fill with.</param>
    /// <returns>The resulting image.</returns>
    public static Image Plain(int width, int height, Pixel pixel)
    {
        var image = new Image(width, height);

        image.Fill(pixel);

        return image;
    }

    /// <summary>
    /// Creates an image filled with the given pixel.
    /// </summary>
    /// <param name="size">The size of the image.</param>
    /// <param name="pixel">The pixel to fill with.</param>
    /// <returns>The resulting image.</returns>
    public static Image Plain(Vec2I size, Pixel pixel)
    {
        return Plain(size.X, size.Y, pixel);
    }

    /// <summary>
    /// Maps the <paramref name="view"/> onto this grid using pixel merging.
    /// </summary>
    /// <param name="view">The view to read from.</param>
    /// <param name="position">The offset on this grid.</param>
    /// <param name="area">The area on the <paramref name="view"/> to map.</param>
    public void MergeMap(IView<Pixel> view, Vec2I position, Rect2DI area)
    {
        area = area.Trim(0, 0, view.Width, view.Height);

        Vec2I start = area.Start - position;

        area = area.Trim(start, Size + start);

        foreach (Vec2I viewPosition in area)
        {
            Vec2I thisPosition = viewPosition - start;
            this[thisPosition] = this[thisPosition].Merge(view[viewPosition]);
        }
    }

    /// <inheritdoc cref="MergeMap(IView{Pixel}, Vec2I, Rect2DI)"/>
    public void MergeMap(IView<Pixel> view, Rect2DI area)
    {
        MergeMap(view, Vec2I.Zero, area);
    }

    /// <inheritdoc cref="MergeMap(IView{Pixel}, Vec2I, Rect2DI)"/>
    public void MergeMap(IView<Pixel> view, Vec2I position)
    {
        MergeMap(view, position, view.Area());
    }

    /// <inheritdoc cref="MergeMap(IView{Pixel}, Vec2I, Rect2DI)"/>
    public void MergeMap(IView<Pixel> view)
    {
        MergeMap(view, Vec2I.Zero);
    }

    /// <summary>
    /// Maps a single line of text onto this image.
    /// </summary>
    /// <param name="line">The text to map.</param>
    /// <param name="x">The position x-component.</param>
    /// <param name="y">The position y-component.</param>
    /// <param name="fgColor">The foreground color to map.</param>
    /// <param name="bgColor">the background color to map.</param>
    /// <returns><see langword="true"/> if any text was mapped; otherwise, <see langword="false"/>.</returns>
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

        int endX = Math.Min(x + line.Length, Width);

        if (endX <= 0)
        {
            return false;
        }

        for (int curX = startX, i = startX - x; curX < endX; curX++, i++)
        {
            this[curX, y] = this[curX, y].Merge(new Pixel(line[i], fgColor, bgColor));
        }

        return true;
    }

    /// <summary>
    /// Maps a single line of text onto this image.
    /// </summary>
    /// <param name="line">The text to map.</param>
    /// <param name="position">The position.</param>
    /// <param name="fgColor">The foreground color to map.</param>
    /// <param name="bgColor">the background color to map.</param>
    /// <returns><see langword="true"/> if any text was mapped; otherwise, <see langword="false"/>.</returns>
    public bool MapLine(string line, Vec2I position, SCEColor fgColor, SCEColor bgColor)
    {
        return MapLine(line, position.X, position.Y, fgColor, bgColor);
    }
}
