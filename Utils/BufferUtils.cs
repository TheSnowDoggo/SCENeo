using SCEBufferAPI;

namespace SCENeo.Utils;

public static class BufferUtils
{
    public static short ToAttributes(ColorInfo colorInfo)
    {
        return (short)((short)colorInfo.ForegroundColor.ToConsoleColor() + ((short)colorInfo.BackgroundColor.ToConsoleColor() << 4));
    }

    public static CharInfo ToCharInfo(Pixel pixel)
    {
        var charInfo = new CharInfo()
        {
            Char = new CharUnion()
            {
                UnicodeChar = pixel.Element,
            },
            Attributes = ToAttributes(pixel.Colors),
        };
        return charInfo;
    }

    public static CharInfo[] ToCharInfoBuffer(Grid2DView<Pixel> grid)
    {
        var buffer = new CharInfo[grid.Size];

        int i = 0;
        for (int y = 0; y < grid.Height; y++)
        {
            for (int x = 0; x < grid.Width; x++)
            {
                buffer[i++] = ToCharInfo(grid[x, y]);
            }
        }

        return buffer;
    }

    public static SmallRect WriteGrid(Grid2DView<Pixel> grid, Coord pos)
    {
        var buf = ToCharInfoBuffer(grid);

        var size = new Coord((short)grid.Width, (short)grid.Height);

        var rect = new SmallRect(pos.X, pos.Y, (short)(pos.X + size.X), (short)(pos.Y + size.Y));

        BufferDrawer.Instance.WriteBuffer(buf, size, pos, ref rect);

        return rect;
    }

    public static SmallRect WriteGrid(Grid2DView<Pixel> grid)
    {
        return WriteGrid(grid, Coord.Zero);
    }
}
