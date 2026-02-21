namespace SCENeo.Serialization.BinImg;

public sealed class BgOnlyEncoding : BinImgEncoding
{
	public override Encoder Encoder => Encoder.BgOnly;

	protected override void SerializeData(Stream stream, IView<Pixel> view, int x, int y)
	{
		stream.WriteByte((byte)view[x, y].BgColor);
	}

	protected override void DeserializeData(Stream stream, Grid2D<Pixel> grid, int x, int y)
	{
		grid[x, y] = new Pixel((SCEColor)stream.ReadExactByte());
	}
}