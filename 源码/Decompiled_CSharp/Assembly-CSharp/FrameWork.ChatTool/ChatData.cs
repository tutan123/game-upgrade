using System;
using Xlsx;

namespace FrameWork.ChatTool;

[Serializable]
public class ChatData
{
	public Xlsx_Message_Key targetId;

	public ChatGraph chatNode;

	public bool isAlwaysEx;
}
