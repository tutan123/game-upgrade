namespace Xlsx;

public class Xlsx_MiNiGameLevel
{
	public string Key;

	public string Title;

	public string Icon;

	public int Target;

	public string[] LoseReward;

	public string[] PassReward;

	public string[] FistReward;

	public long[] FistRewardCount;

	public int Time;

	public string Scene;

	public string Level;

	public int IsHasBqb;

	public string Desc;

	public Xlsx_MiNiGameLevel(string Key, string Title, string Icon, int Target, string[] LoseReward, string[] PassReward, string[] FistReward, long[] FistRewardCount, int Time, string Scene, string Level, int IsHasBqb, string Desc)
	{
		this.Key = Key;
		this.Title = Title;
		this.Icon = Icon;
		this.Target = Target;
		this.LoseReward = LoseReward;
		this.PassReward = PassReward;
		this.FistReward = FistReward;
		this.FistRewardCount = FistRewardCount;
		this.Time = Time;
		this.Scene = Scene;
		this.Level = Level;
		this.IsHasBqb = IsHasBqb;
		this.Desc = Desc;
	}

	public Xlsx_MiNiGameLevel()
	{
	}
}
