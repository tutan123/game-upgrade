namespace Xlsx;

public class Xlsx_RoundInfo
{
	public string Key;

	public string Morning;

	public string Night;

	public Xlsx_RoundInfo(string Key, string Morning, string Night)
	{
		this.Key = Key;
		this.Morning = Morning;
		this.Night = Night;
	}

	public Xlsx_RoundInfo()
	{
	}
}
