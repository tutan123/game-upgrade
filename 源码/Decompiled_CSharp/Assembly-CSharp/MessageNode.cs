using System.Collections.Generic;
using FrameWork.ChatTool;
using XNode;
using Xlsx;

[NodeWidth(250)]
public class MessageNode : Node
{
	public ChatGraph chatGraph;

	public Xlsx_Message_Key key;

	public int minRound;

	public int maxRound;

	public List<PropertyData> properties;

	protected override void Init()
	{
		base.Init();
	}

	public override object GetValue(NodePort port)
	{
		return null;
	}
}
