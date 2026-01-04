using System.Text;

namespace SCENeo;

/// <summary>
/// Itility functions for serializing and deserializing images to a proprietary binary format.
/// </summary>
public static class ImageSerializer
{
    public enum Mode
    {
        Full,
        Opaque,
        BgOnly,
        BgOnlyOpaque,
    }

    private const byte FullSignature = 84;
    private const byte OpaqueSignature = 79;
    private const byte BgOnlySignature = 66;
    private const byte BgOnlyOpaqueSignature = 98;

    public static void Serialize(Stream stream, IView<Pixel> view, Mode encoding = Mode.Full)
    {
        stream.WriteByte(GetSignature(encoding));

        stream.Write(BitConverter.GetBytes(view.Width));
        stream.Write(BitConverter.GetBytes(view.Height));

        switch (encoding)
        {
        case Mode.Full:
            SerializeFull(stream, view);
            break;
        case Mode.Opaque:
            SerializeOpaque(stream, view);
            break;
        case Mode.BgOnly:
            SerializeBgOnly(stream, view);
            break;
        case Mode.BgOnlyOpaque:
            SerializeBgOnlyOpaque(stream, view);
            break;
        }
    }

    public static void Serialize(string path, IView<Pixel> view, Mode encoding = Mode.Full)
    {
        using (var stream = File.Open(path, FileMode.Create))
        {
            Serialize(stream, view, encoding);
        }
    }

    public static Grid2D<Pixel> Deserialize(Stream stream)
    {
        Mode mode = GetEncoding((byte)stream.ReadByte());

        int width  = stream.ReadInt32();
        int height = stream.ReadInt32();

        var grid = new Grid2D<Pixel>(width, height);

        switch (mode)
        {
        case Mode.Full:
            DeserializeFull(grid, stream);
            break;
        case Mode.Opaque:
            DeserializeOpaque(grid, stream);
            break;
        case Mode.BgOnly:
            DeserializeBgOnly(grid, stream);
            break;
        case Mode.BgOnlyOpaque:
            DeserializeBgOnlyOpaque(grid, stream);
            break;
        }

        return grid;
    }

    public static Grid2D<Pixel> Deserialize(string path)
    {
        using (var stream = File.OpenRead(path))
        {
            return Deserialize(stream);
        }
    }

    private static void SerializeFull(Stream stream, IView<Pixel> view)
    {
        char[] buffer = new char[1];

        for (int y = 0; y < view.Height; y++)
        {
            for (int x = 0; x < view.Width; x++)
            {
                Pixel pixel = view[x, y];

                buffer[0] = pixel.Element;
                stream.WriteByte(Encoding.UTF8.GetBytes(buffer)[0]);

                stream.WriteByte((byte)pixel.FgColor);
                stream.WriteByte((byte)pixel.BgColor);
            }
        }
    }

    private static void SerializeOpaque(Stream stream, IView<Pixel> view)
    {
        char[] buffer = new char[1];

        for (int y = 0; y < view.Height; y++)
        {
            for (int x = 0; x < view.Width; x++)
            {
                Pixel pixel = view[x, y];

                buffer[0] = pixel.Element;
                stream.WriteByte(Encoding.UTF8.GetBytes(buffer)[0]);

                stream.WriteByte(PackColors(pixel.FgColor, pixel.BgColor));
            }
        }
    }

    private static void SerializeBgOnly(Stream stream, IView<Pixel> view)
    {
        for (int y = 0; y < view.Height; y++)
        {
            for (int x = 0; x < view.Width; x++)
            {
                stream.WriteByte((byte)view[x, y].BgColor);
            }
        }
    }

    private static void SerializeBgOnlyOpaque(Stream stream, IView<Pixel> view)
    {
        byte lower = 0;
        bool isLower = true;

        for (int y = 0; y < view.Height; y++)
        {
            for (int x = 0; x < view.Width; x++)
            {
                if (isLower)
                {
                    lower = AsOpaque(view[x, y].BgColor);
                }
                else
                {
                    stream.WriteByte((byte)(lower + (AsOpaque(view[x, y].BgColor) << 4)));
                }

                isLower = !isLower;
            }
        }
    }

    private static void DeserializeFull(Grid2D<Pixel> grid, Stream stream)
    {
        byte[] buffer = new byte[1];

        for (int y = 0; y < grid.Height; y++)
        {
            for (int x = 0; x < grid.Width; x++)
            {
                buffer[0] = (byte)stream.ReadByte();
                char element = Encoding.UTF8.GetChars(buffer)[0];

                SCEColor fgColor = (SCEColor)stream.ReadByte();
                SCEColor bgColor = (SCEColor)stream.ReadByte();

                grid[x, y] = new Pixel(element, fgColor, bgColor);
            }
        }
    }

    private static void DeserializeOpaque(Grid2D<Pixel> grid, Stream stream)
    {
        byte[] buffer = new byte[1];

        for (int y = 0; y < grid.Height; y++)
        {
            for (int x = 0; x < grid.Width; x++)
            {
                buffer[0] = (byte)stream.ReadByte();
                char element = Encoding.UTF8.GetChars(buffer)[0];

                UnpackColors((byte)stream.ReadByte(), out SCEColor fgColor, out SCEColor bgColor);

                grid[x, y] = new Pixel(element, fgColor, bgColor);
            }
        }
    }

    private static void DeserializeBgOnly(Grid2D<Pixel> grid, Stream stream)
    {
        for (int y = 0; y < grid.Height; y++)
        {
            for (int x = 0; x < grid.Width; x++)
            {
                SCEColor bgColor = (SCEColor)stream.ReadByte();

                grid[x, y] = new Pixel(bgColor);
            }
        }
    }

    private static void DeserializeBgOnlyOpaque(Grid2D<Pixel> grid, Stream stream)
    {
        byte value = 0;
        bool isLower = true;

        for (int y = 0; y < grid.Height; y++)
        {
            for (int x = 0; x < grid.Width; x++)
            {
                if (isLower)
                {
                    value = (byte)stream.ReadByte();

                    grid[x, y] = new Pixel((SCEColor)(value & 0xF));
                }
                else
                {
                    grid[x, y] = new Pixel((SCEColor)(value >> 4));
                }

                isLower = !isLower;
            }
        }
    }

    private static byte PackColors(SCEColor fgColor, SCEColor bgColor)
    {
        return (byte)(AsOpaque(fgColor) + (AsOpaque(bgColor) << 4));
    }

    private static void UnpackColors(byte packedColors, out SCEColor fgColor, out SCEColor bgColor)
    {
        fgColor = (SCEColor)(packedColors & 0xF);
        bgColor = (SCEColor)(packedColors >> 4);
    }

    private static byte GetSignature(Mode encoding)
    {
        return encoding switch
        {
            Mode.Full         => FullSignature,
            Mode.Opaque       => OpaqueSignature,
            Mode.BgOnly       => BgOnlySignature,
            Mode.BgOnlyOpaque => BgOnlyOpaqueSignature,
            _ => throw new ArgumentOutOfRangeException(nameof(encoding), encoding, "Encoding is invalid.")
        };
    }

    private static Mode GetEncoding(byte signature)
    {
        return signature switch
        {
            FullSignature         => Mode.Full,
            OpaqueSignature       => Mode.Opaque,
            BgOnlySignature       => Mode.BgOnly,
            BgOnlyOpaqueSignature => Mode.BgOnlyOpaque,
            _ => throw new ArgumentOutOfRangeException(nameof(signature), signature, "Signature is invalid.")
        };
    }

    private static byte AsOpaque(SCEColor color)
    {
        return (byte)((int)color & 0xF);
    }

    private static int ReadInt32(this Stream stream)
    {
        Span<byte> span = stackalloc byte[4];

        stream.ReadExactly(span);

        return BitConverter.ToInt32(span);
    }
}