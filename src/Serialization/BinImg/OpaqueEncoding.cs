namespace SCENeo.Serialization.BinImg;

public sealed class OpaqueEncoding : BinImgEncoding
{
	public override Encoder Encoder => Encoder.Opaque;
	
	protected override void SerializeData(Stream stream, IView<Pixel> view, int x, int y)
	{
		Pixel pixel = view[x, y];

		stream.WriteChar(pixel.Element);
		stream.WriteByte((byte)PackColors(pixel.FgColor, pixel.BgColor));
	}

	protected override void DeserializeData(Stream stream, Grid2D<Pixel> grid, int x, int y)
	{
		char element = stream.ReadChar();
		
		UnpackColors((byte)stream.ReadExactByte(), out SCEColor fgColor, out SCEColor bgColor);

		grid[x, y] = new Pixel(element, fgColor, bgColor);
	}
}