namespace Xlsx;

public class Xlsx_Chat
{
	public string Key;

	public string Key2;

	public string MessageKey;

	public string MessageInfo;

	public string[] MessageBtn;

	public string[] MessageBtnInfo;

	public int MsgType;

	public string Desc;

	public Xlsx_Chat(string Key, string Key2, string MessageKey, string MessageInfo, string[] MessageBtn, string[] MessageBtnInfo, int MsgType, string Desc)
	{
		this.Key = Key;
		this.Key2 = Key2;
		this.MessageKey = MessageKey;
		this.MessageInfo = MessageInfo;
		this.MessageBtn = MessageBtn;
		this.MessageBtnInfo = MessageBtnInfo;
		this.MsgType = MsgType;
		this.Desc = Desc;
	}

	public Xlsx_Chat()
	{
	}
}
