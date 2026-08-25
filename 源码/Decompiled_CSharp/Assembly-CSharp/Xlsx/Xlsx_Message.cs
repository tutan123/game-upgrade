namespace Xlsx;

public class Xlsx_Message
{
	public string Key;

	public int Type;

	public string Name;

	public string Icon;

	public string PropertyName;

	public int PropertyMaxValue;

	public string Desc;

	public Xlsx_Message(string Key, int Type, string Name, string Icon, string PropertyName, int PropertyMaxValue, string Desc)
	{
		this.Key = Key;
		this.Type = Type;
		this.Name = Name;
		this.Icon = Icon;
		this.PropertyName = PropertyName;
		this.PropertyMaxValue = PropertyMaxValue;
		this.Desc = Desc;
	}

	public Xlsx_Message()
	{
	}
}
