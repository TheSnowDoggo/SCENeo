namespace SCENeo.Scenes;

public sealed class SceneManager : Scene
{
	public IReadOnlyDictionary<string, Scene> Scenes { get; set; }

	public Scene Get(string name)
	{
		return Scenes[name];
	}

	public T Get<T>(string name)
		where T : Scene
	{
		return (T)Scenes[name];
	}

	public override void Start()
	{
		foreach (var scene in Scenes.Values)
		{
			scene.Start();
		}
	}

	public override void Update(double delta)
	{
		foreach (var scene in Scenes.Values)
		{
			if (!scene.Enabled)
			{
				continue;
			}
			
			scene.Update(delta);
		}
	}

	public override IEnumerable<IRenderable> Render()
	{
		foreach (var scene in Scenes.Values)
		{
			if (!scene.Enabled || !scene.Visible)
			{
				continue;
			}

			foreach (var renderable in scene.Render())
			{
				yield return renderable;
			}
		}
	}

	public override void DisplayResize(Vec2I size)
	{
		foreach (var scene in Scenes.Values)
		{
			scene.DisplayResize(size);
		}
	}

	public override void RawInput(ConsoleKeyInfo cki)
	{
		foreach (var scene in Scenes.Values)
		{
			if (!scene.Enabled)
			{
				continue;
			}
			
			scene.RawInput(cki);
		}
	}
}