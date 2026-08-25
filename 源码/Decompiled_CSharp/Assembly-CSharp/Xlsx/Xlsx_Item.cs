namespace Xlsx;

public class Xlsx_Item
{
	public string Key;

	public string Name;

	public string Info;

	public string AddInfo;

	public new int GetType;

	public int GetMaxCount;

	public int Price;

	public int ItemType;

	public int ZjType;

	public int ZjMode;

	public int YfMode;

	public int MoveSpeed;

	public string[] AddType;

	public int[] AddValue;

	public int IsInfinite;

	public string[] AddItemKeys;

	public int[] AddItemValue;

	public string VideoGroup;

	public string Img;

	public int Range;

	public string Icon;

	public string Desc;

	public Xlsx_Item(string Key, string Name, string Info, string AddInfo, int GetType, int GetMaxCount, int Price, int ItemType, int ZjType, int ZjMode, int YfMode, int MoveSpeed, string[] AddType, int[] AddValue, int IsInfinite, string[] AddItemKeys, int[] AddItemValue, string VideoGroup, string Img, int Range, string Icon, string Desc)
	{
		this.Key = Key;
		this.Name = Name;
		this.Info = Info;
		this.AddInfo = AddInfo;
		this.GetType = GetType;
		this.GetMaxCount = GetMaxCount;
		this.Price = Price;
		this.ItemType = ItemType;
		this.ZjType = ZjType;
		this.ZjMode = ZjMode;
		this.YfMode = YfMode;
		this.MoveSpeed = MoveSpeed;
		this.AddType = AddType;
		this.AddValue = AddValue;
		this.IsInfinite = IsInfinite;
		this.AddItemKeys = AddItemKeys;
		this.AddItemValue = AddItemValue;
		this.VideoGroup = VideoGroup;
		this.Img = Img;
		this.Range = Range;
		this.Icon = Icon;
		this.Desc = Desc;
	}

	public Xlsx_Item()
	{
	}
}
