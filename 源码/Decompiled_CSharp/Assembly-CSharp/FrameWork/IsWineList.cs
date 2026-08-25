using Script.Mrg;
using XNode;
using Xlsx;

namespace FrameWork;

[CreateNodeMenu("Video/IsWineList")]
public class IsWineList : Node
{
	[Input(ShowBackingValue.Unconnected, ConnectionType.Multiple, TypeConstraint.None, false)]
	public VideoNode input;

	[Output(ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.None, false)]
	public VideoNode outputSuccess;

	[Output(ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.None, false)]
	public VideoNode outputFail;

	public Xlsx_WineList_Key wineListKey;

	public bool IsSuc()
	{
		return SingletonAsMono<GameDataMrg>.Instance.xlsxWineListKey == wineListKey;
	}
}
