using System.Text;

namespace SCENeo.Serialization;

/// <summary>
/// Utility functions for serializing and deserializing images to the proprietary SIF format.
/// </summary>
public static class SIFSerializer
{
    private const char Delimiter   = '|';
    private const char EmptySif    = '^';
    private const string Signature = "SIF0";

    public static IReadOnlyBiMap<char, SCEColor> SifCodes { get; } = new BiMap<char, SCEColor>()
    {
        { 'K', SCEColor.Black       },
        { 'B', SCEColor.DarkBlue    },
        { 'G', SCEColor.DarkGreen   },
        { 'C', SCEColor.DarkCyan    },
        { 'R', SCEColor.DarkRed     },
        { 'M', SCEColor.DarkMagenta },
        { 'Y', SCEColor.DarkYellow  },
        { 's', SCEColor.Gray        },
        { 'S', SCEColor.DarkGray    },
        { 'b', SCEColor.Blue        },
        { 'g', SCEColor.Green       },
        { 'c', SCEColor.Cyan        },
        { 'r', SCEColor.Red         },
        { 'm', SCEColor.Magenta     },
        { 'y', SCEColor.Yellow      },
        { 'W', SCEColor.White       },
        { 'N', SCEColor.Transparent },
    };

    /// <summary>
    /// Serializes the image as a SIF to a stream.
    /// </summary>
    /// <param name="stream">The stream to write to.</param>
    /// <param name="image">The image to serialize.</param>
    public static void Serialize(Stream stream, IView<Pixel> image)
    {
        var writer = new StreamWriter(stream, Encoding.UTF8);

        writer.Write(Signature);

        if (image.Width == 0 && image.Height == 0)
        {
            writer.Write(EmptySif);
            writer.Flush();
            return;
        }

        writer.Write($"[{image.Width},{image.Height}]");

        var fgBuilder   = new StringBuilder();
        var bgBuilder   = new StringBuilder();
        var charBuilder = new StringBuilder();

        for (int y = 0; y < image.Height; y++)
        {
            for (int x = 0; x < image.Width; x++)
            {
                Pixel pixel = image[x, y];

                fgBuilder.Append(SifCodes.GetKey1(pixel.FgColor));
                bgBuilder.Append(SifCodes.GetKey1(pixel.BgColor));
                charBuilder.Append(pixel.Element >= ' ' ? pixel.Element : ' ');
            }
        }

        writer.Write(RunLengthEncode(fgBuilder));
        writer.Write('$');

        writer.Write(RunLengthEncode(bgBuilder));
        writer.Write('$');

        writer.Write(RunLengthEncode(charBuilder));
        writer.Write('$');

        writer.Flush();
    }

    /// <summary>
    /// Serializes the image as a SIF to a new file or overwriting an existing file.
    /// </summary>
    /// <param name="filepath">The file to write to.</param>
    /// <param name="image">The image to serialize.</param>
    public static void Serialize(string filepath, IView<Pixel> image)
    {
        using (Stream stream = File.Open(filepath, FileMode.Create))
        {
            Serialize(stream, image);
        }
    }

    /// <summary>
    /// Serializes the image as a SIF as a string.
    /// </summary>
    /// <param name="image">The image to serialize.</param>
    /// <returns>The string SIF.</returns>
    public static string Serialize(IView<Pixel> image)
    {
        using (var stream = new MemoryStream())
        {
            Serialize(stream, image);

            stream.Seek(0, SeekOrigin.Begin);

            var reader = new StreamReader(stream, Encoding.UTF8);

            return reader.ReadToEnd();
        }
    }

    /// <summary>
    /// Deserializes the SIF stream to a grid.
    /// </summary>
    /// <param name="stream">The stream to deserialize.</param>
    /// <returns>The deserialized grid.</returns>
    public static Grid2D<Pixel> Deserialize(Stream stream)
    {
        var reader = new StreamReader(stream, Encoding.UTF8);

        if (reader.ReadLength(Signature.Length) != Signature)
        {
            throw new InvalidDataException($"Expected signature {Signature}.");
        }

        if (reader.EndOfStream)
        {
            throw new InvalidDataException("SIF missing data.");
        }

        if (reader.Peek() == EmptySif)
        {
            return [];
        }

        if (reader.Read() != '[')
        {
            throw new InvalidDataException("Could not find start size delimiter \'[\'.");
        }

        int width  = DeserializeWidth(reader);
        int height = DeserializeHeight(reader);

        int size = width * height;

        var grid = new Grid2D<Pixel>(width, height);

        foreach ((char fgCode, Vec2I pos) in DeserializeSegmentTo(reader, width, size))
        {
            grid[pos] = new Pixel() 
            { 
                FgColor = GetColor(fgCode) 
            };
        }

        foreach ((char bgCode, Vec2I pos) in DeserializeSegmentTo(reader, width, size))
        {
            Pixel pixel = grid[pos];

            grid[pos] = new Pixel() 
            { 
                FgColor = pixel.FgColor,
                BgColor = GetColor(bgCode) 
            };
        }

        foreach ((char element, Vec2I pos) in DeserializeSegmentTo(reader, width, size))
        {
            Pixel pixel = grid[pos];

            grid[pos] = new Pixel()
            {
                FgColor = pixel.FgColor,
                BgColor = pixel.BgColor,
                Element = element,
            };
        }

        return grid;
    }

    /// <summary>
    /// Deserializes the SIF file to a grid.
    /// </summary>
    /// <param name="filepath">The string sif to deserialize.</param>
    /// <returns>The deserialized grid.</returns>
    public static Grid2D<Pixel> Deserialize(string filepath)
    {
        using (var stream = File.OpenRead(filepath))
        {
            return Deserialize(stream);
        }
    }

    /// <summary>
    /// Deserializes the SIF to a grid.
    /// </summary>
    /// <param name="str">The string sif to deserialize.</param>
    /// <returns>The deserialized grid.</returns>
    public static Grid2D<Pixel> DeserializeString(string str)
    {
        using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(str)))
        {
            return Deserialize(stream);
        } 
    }

    private static int DeserializeWidth(StreamReader reader)
    {
        StringBuilder sb = ReadUntil(reader, ',');

        if (reader.EndOfStream)
        {
            throw new InvalidDataException("Could not find size seperator \',\'.");
        }

        string str = sb.ToString();

        if (!int.TryParse(str, out int width) || width < 0)
        {
            throw new InvalidDataException($"Width {str} was invalid.");
        }

        return width;
    }

    private static int DeserializeHeight(StreamReader reader)
    {
        StringBuilder sb = ReadUntil(reader, ']');

        if (reader.EndOfStream)
        {
            throw new InvalidDataException("Could not find end size delimiter \']\'.");
        }

        string str = sb.ToString();

        if (!int.TryParse(str, out int height) || height < 0)
        {
            throw new InvalidDataException($"Height {str} was invalid.");
        }

        return height;
    }

    private static IEnumerable<char> DeserializeSegment(StreamReader reader)
    {
        bool delimited = false;

        while (!reader.EndOfStream && reader.Peek() != '$')
        {
            char current = (char)reader.Read();

            // ignore non-space control characters
            if (current < ' ')
            {
                continue;
            }

            if (current == Delimiter)
            {
                if (delimited && !reader.EndOfStream && reader.Read() == Delimiter)
                {
                    yield return Delimiter;
                }

                delimited = !delimited;
                continue;
            }

            if (delimited || !char.IsDigit(current))
            {
                yield return current;
                continue;
            }

            var countBuilder = new StringBuilder();

            countBuilder.Append(current);

            while (!reader.EndOfStream && char.IsDigit((char)reader.Peek()))
            {
                countBuilder.Append((char)reader.Read());
            }

            if (reader.EndOfStream)
            {
                throw new InvalidDataException("Run count missing associated character.");
            }

            string countStr = countBuilder.ToString();

            if (!int.TryParse(countStr, out int count) || count < 0)
            {
                throw new InvalidDataException($"Count \'{countStr}\' was invalid.");
            }

            current = (char)reader.Read();

            if (current == Delimiter)
            {
                if (reader.EndOfStream)
                {
                    throw new InvalidDataException("Delimiter must be followed by a character.");
                }

                current = (char)reader.Read();
            }

            for (int i = 0; i < count; i++)
            {
                yield return current;
            }
        }

        if (reader.EndOfStream)
        {
            throw new InvalidDataException("Segment missing end delimiter \'$\'.");
        }

        reader.Read();
    }

    private static IEnumerable<(char, Vec2I)> DeserializeSegmentTo(StreamReader reader, int width, int size)
    {
        int i = 0;

        foreach (char c in DeserializeSegment(reader))
        {
            if (i == size)
            {
                throw new InvalidDataException($"More data than expected.");
            }

            yield return (c, new Vec2I(i % width, i / width));

            i++;
        }

        if (i != size)
        {
            throw new InvalidDataException($"Too little data than expected.");
        }
    }

    private static StringBuilder ReadUntil(StreamReader reader, char end)
    {
        var sb = new StringBuilder();

        while (!reader.EndOfStream && reader.Peek() != end)
        {
            sb.Append((char)reader.Read());
        }

        reader.Read();

        return sb;
    }

    private static string ReadLength(this StreamReader reader, int length)
    {
        Span<char> span = stackalloc char[length];

        reader.ReadBlock(span);

        return new string(span);
    }

    private static SCEColor GetColor(char code)
    {
        if (!SifCodes.TryGetKey2(code, out SCEColor color))
        {
            throw new InvalidDataException($"Unrecognised color code {code}.");
        }
        return color;
    }

    private static StringBuilder RunLengthEncode(StringBuilder source)
    {
        var sb = new StringBuilder();

        if (source.Length == 0)
        {
            return sb;
        }

        char last = source[0];
        int count = 1;

        for (int i = 1; i < source.Length; i++)
        {
            if (source[i] == last)
            {
                count++;
                continue;
            }

            AppendEncode(sb, last, count);

            last  = source[i];
            count = 1;
        }

        AppendEncode(sb, last, count);

        return sb;
    }

    private static void AppendSingle(StringBuilder sb, char c)
    {
        if (c == Delimiter)
        {
            sb.Append(Delimiter, 2);
            return;
        }

        if (!char.IsDigit(c))
        {
            sb.Append(c);
            return;
        }

        sb.Append($"{Delimiter}{c}{Delimiter}");
    }

    private static void AppendEncode(StringBuilder sb, char c, int count)
    {
        if (count == 1)
        {
            AppendSingle(sb, c);
            return;
        } 

        if (!char.IsDigit(c) && c != Delimiter)
        {
            sb.Append($"{count}{c}");
            return;
        }

        sb.Append($"{count}{Delimiter}{c}");
    }
}