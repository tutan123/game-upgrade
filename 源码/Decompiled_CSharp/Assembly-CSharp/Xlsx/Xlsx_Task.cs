namespace Xlsx;

public class Xlsx_Task
{
	public string Key;

	public string Title;

	public string Info;

	public int NotShowTask;

	public int IsShowSy;

	public int IsMain;

	public int Round;

	public string[] PropertyTypeShow;

	public int[] PropertyTypeValueShow;

	public string[] PropertyType;

	public int[] PropertyTypeValue;

	public int Range;

	public string Desc;

	public Xlsx_Task(string Key, string Title, string Info, int NotShowTask, int IsShowSy, int IsMain, int Round, string[] PropertyTypeShow, int[] PropertyTypeValueShow, string[] PropertyType, int[] PropertyTypeValue, int Range, string Desc)
	{
		this.Key = Key;
		this.Title = Title;
		this.Info = Info;
		this.NotShowTask = NotShowTask;
		this.IsShowSy = IsShowSy;
		this.IsMain = IsMain;
		this.Round = Round;
		this.PropertyTypeShow = PropertyTypeShow;
		this.PropertyTypeValueShow = PropertyTypeValueShow;
		this.PropertyType = PropertyType;
		this.PropertyTypeValue = PropertyTypeValue;
		this.Range = Range;
		this.Desc = Desc;
	}

	public Xlsx_Task()
	{
	}
}
