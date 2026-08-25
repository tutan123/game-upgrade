using Script.Mrg;
using XNode;

namespace FrameWork;

[CreateNodeMenu("Video/VideoIfTargetEbriety")]
public class VideoIfTargetEbriety : Node
{
	[Input(ShowBackingValue.Unconnected, ConnectionType.Multiple, TypeConstraint.None, false)]
	public VideoNode input;

	[Output(ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.None, false)]
	public VideoNode outputSuccess;

	[Output(ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.None, false)]
	public VideoNode outputFail;

	public bool IsSuc()
	{
		return SingletonAsMono<GameDataMrg>.Instance.TmpTargetJiuLian >= SingletonAsMono<GameDataMrg>.Instance.TargetJiuLian;
	}
}
