namespace SCENeo.Ui;

public abstract class UiBaseDimensioned : UiBase, IDimensioned
{
	protected bool _update;
	
	private int _width;

	/// <summary>
	/// Gets or sets the width.
	/// </summary>
	public int Width
	{
		get => _width;
		set => ObserveSet(ref _width, value);
	}

	private int _height;

	/// <summary>
	/// Gets or sets the height.
	/// </summary>
	public int Height
	{
		get => _height;
		set => ObserveSet(ref _height, value);
	}

	protected void ObserveSet<T>(ref T property, T value)
	{
		ObserveSet(ref property, value, ref _update);
	}
	
	protected void ObserveSet<T>(ref T property, T value, Action updateCallback)
		where T : IUpdate
	{
		ObserveSet(ref property, value, ref _update, updateCallback);
	}
}