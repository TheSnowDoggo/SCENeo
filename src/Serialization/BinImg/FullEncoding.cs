namespace SCENeo.Serialization.BinImg;

public sealed class FullEncoding : BinImgEncoding
{
	public override Encoder Encoder => Encoder.Full;
	
	protected override void SerializeData(Stream stream, IView<Pixel> view, int x, int y)
	{
		Pixel pixel = view[x, y];

		stream.WriteByte((byte)pixel.Element);
		stream.WriteByte((byte)pixel.FgColor);
		stream.WriteByte((byte)pixel.BgColor);
	}

	protected override void DeserializeData(Stream stream, Grid2D<Pixel> grid, int x, int y)
	{
		grid[x, y] = new Pixel()
		{
			Element = (char)stream.ReadExactByte(),
			FgColor = (SCEColor)stream.ReadExactByte(),
			BgColor = (SCEColor)stream.ReadExactByte(),
		};
	}
}