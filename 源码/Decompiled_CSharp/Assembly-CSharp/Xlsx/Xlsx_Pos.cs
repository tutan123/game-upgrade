namespace Xlsx;

public class Xlsx_Pos
{
	public int Key;

	public int MaxRoundCount;

	public string Desc;

	public Xlsx_Pos(int Key, int MaxRoundCount, string Desc)
	{
		this.Key = Key;
		this.MaxRoundCount = MaxRoundCount;
		this.Desc = Desc;
	}

	public Xlsx_Pos()
	{
	}
}
