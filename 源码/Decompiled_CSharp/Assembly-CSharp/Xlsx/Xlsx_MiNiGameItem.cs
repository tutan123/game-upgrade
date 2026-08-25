namespace Xlsx;

public class Xlsx_MiNiGameItem
{
	public string Key;

	public int Price;

	public float Add;

	public int MaxGet;

	public string TouchDesc;

	public string Desc;

	public Xlsx_MiNiGameItem(string Key, int Price, float Add, int MaxGet, string TouchDesc, string Desc)
	{
		this.Key = Key;
		this.Price = Price;
		this.Add = Add;
		this.MaxGet = MaxGet;
		this.TouchDesc = TouchDesc;
		this.Desc = Desc;
	}

	public Xlsx_MiNiGameItem()
	{
	}
}
