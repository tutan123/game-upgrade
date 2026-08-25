using System.Collections.Generic;
using FrameWork;
using XNode;

[CreateNodeMenu("Video/VideoIf")]
[NodeWidth(300)]
public class VideoIf : Node
{
	[Input(ShowBackingValue.Unconnected, ConnectionType.Multiple, TypeConstraint.None, false)]
	public string input;

	public List<PropertyData> properties = new List<PropertyData>();

	[Output(ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.None, false)]
	public VideoNode oneVideoNode;

	[Output(ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.None, false)]
	public VideoNode towVideoNode;

	protected override void Init()
	{
		base.Init();
	}

	public override object GetValue(NodePort port)
	{
		return null;
	}

	public VideoNode GetVideoNode()
	{
		if (properties.IsSuc())
		{
			List<VideoNode> allVideoNodes = GetOutputPort("oneVideoNode").GetAllVideoNodes();
			if (allVideoNodes.Count > 0)
			{
				return allVideoNodes[0];
			}
			return null;
		}
		List<VideoNode> allVideoNodes2 = GetOutputPort("towVideoNode").GetAllVideoNodes();
		if (allVideoNodes2.Count > 0)
		{
			return allVideoNodes2[0];
		}
		return null;
	}
}
