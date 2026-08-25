namespace Xlsx;

public class Xlsx_Chapter
{
	public string Key;

	public int Type;

	public string RoleKey;

	public Xlsx_Chapter(string Key, int Type, string RoleKey)
	{
		this.Key = Key;
		this.Type = Type;
		this.RoleKey = RoleKey;
	}

	public Xlsx_Chapter()
	{
	}
}
