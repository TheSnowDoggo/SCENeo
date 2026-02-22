namespace SCENeo.Ui;

public class TabItem : ListBoxItem
{
	public const int AutoWidth = -1;

	private bool? _fitToHeight;

	public bool? FitToHeight
	{
		get => _fitToHeight;
		set => ObserveSet(ref _fitToHeight, value);
	}
	
	private int? _minWidth;

	public int? MinWidth
	{
		get => _minWidth;
		set => ObserveSet(ref _minWidth, value);
	}

	private TextWrapping? _wrapping;

	public TextWrapping? Wrapping
	{
		get => _wrapping;
		set => ObserveSet(ref _wrapping, value);
	}
	
	public int GetMinWidth()
	{
		if (MinWidth is { } minWidth)
		{
			return minWidth;
		}
		
		return Inherited is TabItem { MinWidth: not null } tabItem ? tabItem.MinWidth.Value : AutoWidth;
	}
	
	public TextWrapping GetWrapping()
	{
		if (Wrapping is { } wrapping)
		{
			return wrapping;
		}
		
		return Inherited is TabItem { Wrapping: not null } tabItem ? tabItem.Wrapping.Value : TextWrapping.None;
	}
	
	public bool GetFitToHeight()
	{
		if (FitToHeight is { } fitToHeight)
		{
			return fitToHeight;
		}
		
		return Inherited is TabItem { FitToHeight: not null } tabItem ? tabItem.FitToHeight.Value : false;
	}
	
	public new UpdateList<TabItem> FromTemplate(IList<string> lines)
	{
		var list = new UpdateList<TabItem>(lines.Count);

		foreach (string text in lines)
		{
			list.Add(FromTemplate(text));
		}

		return list;
	}

	public new TabItem FromTemplate(string text) => new TabItem()
	{
		Inherited = this,
		Text = text,
	};
}