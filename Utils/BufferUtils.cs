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

    public static SmallRect WriteGrid(Grid2DView<Pixel> grid, Vec2I position)
    {
        CharInfo[] buf = ToCharInfoBuffer(grid);
        Coord size     = new Coord((short)grid.Width, (short)grid.Height);
        Coord pos      = new Coord((short)position.X, (short)position.Y);
        SmallRect rect = new SmallRect(pos.X, pos.Y, (short)(pos.X + size.X), (short)(pos.Y + size.Y));

        BufferDrawer.Instance.WriteBuffer(buf, size, pos, ref rect);

        return rect;
    }

    public static SmallRect WriteGrid(Grid2DView<Pixel> grid)
    {
        return WriteGrid(grid, Vec2I.Zero);
    }
}
