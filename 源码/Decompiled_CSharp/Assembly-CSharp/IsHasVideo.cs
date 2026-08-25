using System.Collections.Generic;
using FrameWork;
using Script.UiTool;
using XNode;

[CreateNodeMenu("Video/IsHasVideo")]
[NodeWidth(300)]
public class IsHasVideo : Node
{
	[Input(ShowBackingValue.Unconnected, ConnectionType.Multiple, TypeConstraint.None, false)]
	public string input;

	public List<VideoUnlockData> videoUnlockDatas = new List<VideoUnlockData>();

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
		if (videoUnlockDatas.IsUnLockAllVideo())
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
