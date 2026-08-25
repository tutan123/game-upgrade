using FrameWork;
using Script.Mrg;
using XNode;

[CreateNodeMenu("Video/VideoIfAsType")]
[NodeWidth(300)]
public class VideoIfAsType : Node
{
	[Input(ShowBackingValue.Unconnected, ConnectionType.Multiple, TypeConstraint.None, false)]
	public string input;

	public PropertyTypeValue type1;

	public PropertyTypeValue type2;

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
		if (SingletonAsMono<GameDataMrg>.Instance.GetProperty(type1.ToString(), "Property", 0L) >= SingletonAsMono<GameDataMrg>.Instance.GetProperty(type2.ToString(), "Property", 0L))
		{
			return GetOutputPort("oneVideoNode").GetAllVideoNodes()[0];
		}
		return GetOutputPort("towVideoNode").GetAllVideoNodes()[0];
	}
}
