namespace Xlsx;

public class Xlsx_Event
{
	public string Key;

	public int Type;

	public int[] Round;

	public int[] IsNotRound;

	public int IsMorning;

	public int Count;

	public int AutoSave;

	public int NotAutoSave;

	public string RoleIcon;

	public int IsInfinitize;

	public int IsNotShowMap;

	public int IsShowRound;

	public int IsDemo;

	public string ShowText;

	public string VideoAsset;

	public string Desc;

	public Xlsx_Event(string Key, int Type, int[] Round, int[] IsNotRound, int IsMorning, int Count, int AutoSave, int NotAutoSave, string RoleIcon, int IsInfinitize, int IsNotShowMap, int IsShowRound, int IsDemo, string ShowText, string VideoAsset, string Desc)
	{
		this.Key = Key;
		this.Type = Type;
		this.Round = Round;
		this.IsNotRound = IsNotRound;
		this.IsMorning = IsMorning;
		this.Count = Count;
		this.AutoSave = AutoSave;
		this.NotAutoSave = NotAutoSave;
		this.RoleIcon = RoleIcon;
		this.IsInfinitize = IsInfinitize;
		this.IsNotShowMap = IsNotShowMap;
		this.IsShowRound = IsShowRound;
		this.IsDemo = IsDemo;
		this.ShowText = ShowText;
		this.VideoAsset = VideoAsset;
		this.Desc = Desc;
	}

	public Xlsx_Event()
	{
	}
}
