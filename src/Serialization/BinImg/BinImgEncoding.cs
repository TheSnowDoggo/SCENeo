using System.ComponentModel;
using System.Runtime.Serialization;

namespace SCENeo.Serialization.BinImg;

public abstract class BinImgEncoding
{
	public static FullEncoding Full { get; } = new FullEncoding();
	public static OpaqueEncoding Opaque { get; } = new OpaqueEncoding();
	public static BgOnlyEncoding BgOnly { get; } = new BgOnlyEncoding();
	public static BgOnlyOpaqueEncoding BgOnlyOpaque { get; } = new BgOnlyOpaqueEncoding();

	public static void SerializeAs(Stream stream, IView<Pixel> view, Encoder encoder)
	{
		switch (encoder)
		{
		case Encoder.Full:
			Full.Serialize(stream, view);
			break;
		case Encoder.Opaque:
			Opaque.Serialize(stream, view);
			break;
		case Encoder.BgOnly:
			BgOnly.Serialize(stream, view);
			break;
		case Encoder.BgOnlyOpaque:
			BgOnlyOpaque.Serialize(stream, view);
			break;
		default:
			throw new ArgumentOutOfRangeException(nameof(encoder), encoder, $"Encoder {encoder} is not supported.");
		}
	}
	
	public static void SerializeAs(string filepath, IView<Pixel> view, Encoder encoder, FileMode fileMode = FileMode.Create)
	{
		using var fs = File.Open(filepath, fileMode);
		SerializeAs(fs, view, encoder);
	}
	
	public static Grid2D<Pixel> DetectDeserialize(Stream stream)
	{
		return DetectEncoding(stream).Deserialize(stream);
	}
	
	public static Grid2D<Pixel> DetectDeserialize(string filepath)
	{
		using var fs = File.OpenRead(filepath);
		return DetectDeserialize(fs);
	}
	
	public static BinImgEncoding DetectEncoding(Stream stream)
	{
		Encoder encoder = (Encoder)stream.PeakExactByte();
			
		return encoder switch
		{
			Encoder.Full => Full,
			Encoder.Opaque => Opaque,
			Encoder.BgOnly => BgOnly,
			Encoder.BgOnlyOpaque => BgOnlyOpaque,
			_ => throw new InvalidDataException($"Unrecognised image encoding: {encoder}"),
		};
	}

	public void Serialize(Stream stream, IView<Pixel> view)
	{
		stream.WriteByte((byte)Encoder);
		
		int width = view.Width;
		
		if (width < 0)
		{
			throw new ArgumentOutOfRangeException(nameof(view), width, "View width is negative.");
		}
		
		stream.WriteInt32(width);
		
		int height = view.Height;
		
		if (height < 0)
		{
			throw new ArgumentOutOfRangeException(nameof(view), height, "View height is negative.");
		}
		
		stream.WriteInt32(height);
		
		SerializeData(stream, view);
	}

	public void Serialize(string filename, IView<Pixel> view, FileMode fileMode = FileMode.Create)
	{
		using var fs = File.Open(filename, fileMode);
		Serialize(fs, view);
	}

	public Grid2D<Pixel> Deserialize(Stream stream)
	{
		Encoder encoder = (Encoder)stream.ReadExactByte();

		if (encoder != Encoder)
		{
			throw new InvalidDataException($"Encoder {encoder} is not supported by current encoding.");
		}
		
		int width = stream.ReadInt32();

		if (width < 0)
		{
			throw new InvalidDataContractException($"Width {width} is negative.");
		}

		int height = stream.ReadInt32();

		if (height < 0)
		{
			throw new InvalidDataException($"Height {height} is negative.");
		}
		
		var grid = new Grid2D<Pixel>(width, height);
		
		DeserializeData(stream, grid);
		
		return grid;
	}
	
	public Grid2D<Pixel> Deserialize(string filename)
	{
		using var fs = File.OpenRead(filename);
		return Deserialize(fs);
	}

	public abstract Encoder Encoder { get; }

	protected virtual void SerializeData(Stream stream, IView<Pixel> view)
	{
		for (int y = 0; y < view.Height; y++)
		{
			for (int x = 0; x < view.Width; x++)
			{
				SerializeData(stream, view, x, y);
			}
		}
	}

	protected virtual void SerializeData(Stream stream, IView<Pixel> view, int x, int y)
	{
	}

	protected virtual void DeserializeData(Stream stream, Grid2D<Pixel> grid)
	{
		for (int y = 0; y < grid.Height; y++)
		{
			for (int x = 0; x < grid.Width; x++)
			{
				DeserializeData(stream, grid, x, y);
			}
		}
	}

	protected virtual void DeserializeData(Stream stream, Grid2D<Pixel> grid, int x, int y)
	{
	}
	
	protected static int PackColors(SCEColor fgColor, SCEColor bgColor)
	{
		return ToOpaque(fgColor) + (ToOpaque(bgColor) << 4);
	}

	protected static void UnpackColors(byte packedColors, out SCEColor fgColor, out SCEColor bgColor)
	{
		fgColor = (SCEColor)(packedColors & 0xF);
		bgColor = (SCEColor)(packedColors >> 4);
	}
	
	protected static int ToOpaque(SCEColor color)
	{
		return (int)color & 0xF;
	}
}