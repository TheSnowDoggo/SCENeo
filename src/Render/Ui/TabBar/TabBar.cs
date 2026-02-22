using System.ComponentModel;

namespace SCENeo.Ui;

public sealed class TabBar : UiBaseDimensioned, IRenderable
{
	private readonly Image _buffer = new Image();
	
	private UpdateList<TabItem> _items;

	public UpdateList<TabItem> Items
	{
		get => _items;
		set => ObserveSet(ref _items, value, () => _update = true);
	}
	
	private Pixel _basePixel = new Pixel(SCEColor.Gray, SCEColor.Black);

	public Pixel BasePixel
	{
		get => _basePixel;
		set => ObserveSet(ref _basePixel, value);
	}
	
	private int _selected;

	public int Selected
	{
		get => _selected;
		set => ObserveSet(ref _selected, value);
	}

	private SlideMode _slideMode;

	public SlideMode SlideMode
	{
		get => _slideMode;
		set => ObserveSet(ref _slideMode, value);
	}

	public IView<Pixel> Render()
	{
		if (!_update)
		{
			return _buffer;
		}

		_update = false;

		_buffer.TryCleanResize(Width, Height);
		_buffer.Fill(BasePixel);

		int x = 0;
		
		for (int i = 0; i < Items.Count; i++)
		{
			if (x >= Width)
			{
				break;
			}
			
			var item = Items[i];

			if (item == null)
			{
				continue;
			}
			
			bool selected = i == _selected;
			
			string text = item.GetText();
			
			int minWidth = item.GetMinWidth();
			int width = minWidth < 0 ? text.Length : minWidth;

			var textMapper = new TextMapper()
			{
				Anchor = item.GetAnchor(),
				FitToLength = item.GetFitToLength(),
				FitToHeight = item.GetFitToHeight(),
				Wrapping = item.GetWrapping(),
				FgColor = item.GetFgColor(selected),
				BgColor = item.GetBgColor(selected),
				MaxWidth = width,
				StartOffset = new Vec2I(Translate(x, width), 0),
			};

			textMapper.MapText(_buffer, text);
			
			x += width;
		}
		
		return _buffer;
	}

	private int Translate(int x, int width) => SlideMode switch
	{
		SlideMode.LeftRight => x,
		SlideMode.RightLeft => Width - x - width - 1,
		_ => throw new InvalidEnumArgumentException(nameof(SlideMode), (int)SlideMode, typeof(SlideMode)),
	};
}