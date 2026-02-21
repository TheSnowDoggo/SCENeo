namespace SCENeo.Serialization.BinImg;

public sealed class BgOnlyOpaqueEncoding : BinImgEncoding
{
	public override Encoder Encoder => Encoder.BgOnlyOpaque;
	
	protected override void SerializeData(Stream stream, IView<Pixel> view)
	{
		int width = view.Width;
		int length = width * view.Height;

		for (int i = 0; i < length; i += 2)
		{
			if (i == length - 1)
			{
				stream.WriteByte((byte)((int)view[i % width, i / width].BgColor & 0xF));
				continue;
			}

			int value = PackColors(view[i % width, i / width].BgColor, view[(i + 1) % width, (i + 1) / width].BgColor);
			
			stream.WriteByte((byte)value);
		}
	}

	protected override void DeserializeData(Stream stream, Grid2D<Pixel> grid)
	{
		int width = grid.Width;
		int length = grid.Length;

		for (int i = 0; i < length; i += 2)
		{
			if (i == length - 1)
			{
				grid[i % width, i / width] = new Pixel((SCEColor)(stream.ReadExactByte() & 0xF));
				continue;
			}
			
			UnpackColors(stream.ReadExactByte(), out SCEColor bg1, out SCEColor bg2);

			grid[i % width, i / width] = new Pixel(bg1);
			grid[(i + 1) % width, (i + 1) / width] = new Pixel(bg2);
		}
	}
}