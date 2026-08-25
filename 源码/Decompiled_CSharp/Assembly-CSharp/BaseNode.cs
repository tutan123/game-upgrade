using System;
using System.Linq;
using XNode;

public class BaseNode : Node
{
	public string uniqueID;

	protected override void Init()
	{
		base.Init();
		UpdateIDInGraph();
	}

	private void UpdateIDInGraph()
	{
		bool flag = string.IsNullOrEmpty(uniqueID);
		if (!flag)
		{
			flag = graph.nodes.Any((Node n) => n != null && n != this && n is BaseNode baseNode && baseNode.uniqueID == uniqueID);
		}
		if (flag)
		{
			uniqueID = Guid.NewGuid().ToString();
		}
	}
}
