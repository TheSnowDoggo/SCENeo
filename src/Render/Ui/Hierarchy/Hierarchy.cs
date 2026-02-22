using System.Text;

namespace SCENeo.Ui;

public sealed class Hierarchy : UiBaseDimensioned, IRenderable
{
	private sealed record LocatedNode(HierarchyNode Node, int Level);
	
	private readonly Image _buffer = new Image();
	
	private HierarchyNode _root;

	public event Action RenderSelectionChanged;
	
	public HierarchyNode Root
	{
		get => _root;
		set => ObserveSet(ref _root, value, () => _update = true);
	}

	private Pixel _basePixel;

	public Pixel BasePixel
	{
		get => _basePixel;
		set => ObserveSet(ref _basePixel, value);
	}

	private int _scroll;

	public int Scroll
	{
		get => _scroll;
		set => ObserveSet(ref _scroll, value);
	}

	private int _selected;

	public int Selected
	{
		get => _selected;
		set => ObserveSet(ref _selected, value); 
	}

	private HierarchyNode _lastRenderSelected;

	public HierarchyNode LastRenderedSelected
	{
		get => _lastRenderSelected;
		private set
		{
			if (_lastRenderSelected == value)
			{
				return;
			}
			
			_lastRenderSelected = value;
			RenderSelectionChanged?.Invoke();
		}
	}
	
	private char _levelFill = ' ';

	public char LevelFill
	{
		get => _levelFill;
		set => ObserveSet(ref _levelFill, value);
	}

	private int _levelMultiplier = 2;

	public int LevelMultiplier
	{
		get => _levelMultiplier;
		set => ObserveSet(ref _levelMultiplier, value);
	}

	private bool _fitToLength;

	public bool FitToLength
	{
		get => _fitToLength;
		set => ObserveSet(ref _fitToLength, value);
	}

	private bool _highlightNullSelected;

	public bool HighlightNullSelected
	{
		get => _highlightNullSelected;
		set => ObserveSet(ref _highlightNullSelected, value);
	}

	private string _openSuffix  = " v";

	public string OpenSuffix
	{
		get => _openSuffix;
		set => ObserveSet(ref _openSuffix, value);
	}

	private string _closeSuffix = " >";
	
	public string CloseSuffix
	{
		get => _closeSuffix;
		set => ObserveSet(ref _closeSuffix, value);
	}
	
	private SCEColor _selectedFgColor = SCEColor.Black;

	public SCEColor SelectedFgColor
	{
		get => _selectedFgColor;
		set => ObserveSet(ref _selectedFgColor, value);
	}

	private SCEColor _selectedBgColor = SCEColor.White;

	public SCEColor SelectedBgColor
	{
		get => _selectedBgColor;
		set => ObserveSet(ref _selectedBgColor, value);
	}

	private SCEColor _unselectedFgColor = SCEColor.Gray;

	public SCEColor UnselectedFgColor
	{
		get => _unselectedFgColor;
		set => ObserveSet(ref _unselectedFgColor, value);
	}

	private SCEColor _unselectedBgColor = SCEColor.Black;

	public SCEColor UnselectedBgColor
	{
		get => _unselectedBgColor;
		set => ObserveSet(ref _unselectedBgColor, value);
	}

	public IView<Pixel> Render()
	{
		if (!_update)
		{
			return _buffer;
		}

		_update = false;

		_buffer.TryCleanResize(Width, Height);

		List<LocatedNode> visible = GetVisibleNodes();

		for (int y = 0; y < _buffer.Height; y++)
		{
			int i = y + _scroll;
			
			bool selected = i == _selected;

			var fgColor = selected ? _selectedFgColor : _unselectedFgColor;
			var bgColor = selected ? _selectedBgColor : _unselectedBgColor;
			
			if (i < 0 || i >= visible.Count)
			{
				Pixel fill = _highlightNullSelected ? new Pixel(' ', fgColor, bgColor) : _basePixel;
				_buffer.Fill(fill, 0, y, _buffer.Width, y + 1);

				if (selected)
				{
					LastRenderedSelected = null;
				}
				continue;
			}

			var node = visible[i];

			if (selected)
			{
				LastRenderedSelected = node.Node;
			}
			
			var sb = new StringBuilder();
			sb.Append(_levelFill, node.Level * _levelMultiplier);
			sb.Append(node.Node.Text);

			if (node.Node.Children is { Count: > 0 })
			{
				sb.Append(node.Node.Open ? _openSuffix : _closeSuffix);
			}

			string text = sb.ToString();

			var textMapper = new TextMapper()
			{
				FitToLength = _fitToLength,
				FgColor = fgColor,
				BgColor = bgColor,
				StartOffset = new Vec2I(0, y),
			};
			
			textMapper.MapText(_buffer, text);
		}
		
		return _buffer;
	}

	private List<LocatedNode> GetVisibleNodes()
	{
		if (Root is not { Visible: true })
		{
			return [];
		}
		
		var list = new List<LocatedNode>();

		var stack = new Stack<LocatedNode>();
		stack.Push(new LocatedNode(Root, 0));

		var seen = new HashSet<HierarchyNode>() { Root };

		while (stack.TryPop(out LocatedNode lNode))
		{
			list.Add(lNode);

			var node = lNode.Node;
			if (node.Children == null || !node.Open)
			{
				continue;
			}

			foreach (var child in node.Children)
			{
				if (!seen.Add(child))
				{
					throw new InvalidOperationException($"Node {node} has recursive child {child}.");
				}
				
				if (child is not { Visible: true })
				{
					continue;
				}
				
				stack.Push(new LocatedNode(child, lNode.Level + 1));
			}
		}

		return list;
	}
}