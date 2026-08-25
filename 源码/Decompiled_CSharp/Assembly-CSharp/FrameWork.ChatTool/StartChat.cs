using XNode;
using Xlsx;

namespace FrameWork.ChatTool;

[CreateNodeMenu("Chat/StartChat")]
public class StartChat : Node
{
	[Output(ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.None, false)]
	public string outPut;

	public int maxRound = -1;

	public Xlsx_Language_Key xlsxLanguageKey;
}
