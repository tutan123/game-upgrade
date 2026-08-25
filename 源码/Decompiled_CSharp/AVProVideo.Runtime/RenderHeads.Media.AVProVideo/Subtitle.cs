namespace RenderHeads.Media.AVProVideo;

public class Subtitle
{
	public int index;

	public string text;

	public double timeStart;

	public double timeEnd;

	public bool IsBefore(double time)
	{
		if (time > timeStart)
		{
			return time > timeEnd;
		}
		return false;
	}

	public bool IsTime(double time)
	{
		if (time >= timeStart)
		{
			return time < timeEnd;
		}
		return false;
	}
}
