namespace Xlsx;

public class Xlsx_Record
{
	public string Key;

	public int Type;

	public string EventKey;

	public Xlsx_Record(string Key, int Type, string EventKey)
	{
		this.Key = Key;
		this.Type = Type;
		this.EventKey = EventKey;
	}

	public Xlsx_Record()
	{
	}
}
