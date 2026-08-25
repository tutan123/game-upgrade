namespace Xlsx;

public class Xlsx_WineList
{
	public string Key;

	public string WineListName;

	public int ZJAdd;

	public int JLAdd;

	public int JLBFBAdd;

	public int HGAdd;

	public int QAdd;

	public int XDL;

	public string Info;

	public string Desc;

	public Xlsx_WineList(string Key, string WineListName, int ZJAdd, int JLAdd, int JLBFBAdd, int HGAdd, int QAdd, int XDL, string Info, string Desc)
	{
		this.Key = Key;
		this.WineListName = WineListName;
		this.ZJAdd = ZJAdd;
		this.JLAdd = JLAdd;
		this.JLBFBAdd = JLBFBAdd;
		this.HGAdd = HGAdd;
		this.QAdd = QAdd;
		this.XDL = XDL;
		this.Info = Info;
		this.Desc = Desc;
	}

	public Xlsx_WineList()
	{
	}
}
