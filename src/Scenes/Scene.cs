namespace SCENeo.Scenes;

public abstract class Scene
{
	public bool Enabled { get; set; } = true;
	public bool Visible { get; set; } = true;
	
	public virtual void Start()
	{
	}
	
	public virtual void Update(double delta)
	{
	}

	public virtual void DisplayResize(Vec2I size)
	{
	}

	public virtual IEnumerable<IRenderable> Render()
	{
		yield break;
	}

	public virtual void RawInput(ConsoleKeyInfo cki)
	{
	}
}