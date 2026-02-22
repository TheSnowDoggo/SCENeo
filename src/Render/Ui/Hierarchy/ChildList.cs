namespace SCENeo.Ui;

public class ChildList : UpdateList<HierarchyNode>
{
	public HierarchyNode Parent { get; set; }
	
	public override void Add(HierarchyNode item)
	{
		base.Add(item);

		if (item != null)
		{
			item.Parent = Parent;
		}
	}

	public override void RemoveAt(int index)
	{
		var node = this[index];
		
		base.RemoveAt(index);

		if (node != null)
		{
			node.Parent = null;
		}
	}
}