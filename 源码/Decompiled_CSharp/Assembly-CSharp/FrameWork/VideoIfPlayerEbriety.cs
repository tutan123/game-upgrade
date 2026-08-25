using Script.Mrg;
using XNode;

namespace FrameWork;

[CreateNodeMenu("Video/VideoIfPlayerEbriety")]
public class VideoIfPlayerEbriety : Node
{
	[Input(ShowBackingValue.Unconnected, ConnectionType.Multiple, TypeConstraint.None, false)]
	public VideoNode input;

	[Output(ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.None, false)]
	public VideoNode outputSuccess;

	[Output(ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.None, false)]
	public VideoNode outputFail;

	public bool IsSuc()
	{
		return SingletonAsMono<GameDataMrg>.Instance.TmpPlayerJiuLian >= SingletonAsMono<GameDataMrg>.Instance.PlayerJiuLian;
	}
}
