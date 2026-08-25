using System.Runtime.InteropServices;

namespace RenderHeads.Media.AVProVideo;

[StructLayout(0, Pack = 1)]
public struct TimeRange
{
	public double startTime;

	public double duration;

	public double StartTime => startTime;

	public double EndTime => startTime + duration;

	public double Duration => duration;

	public TimeRange(double startTime, double duration)
	{
		this.startTime = startTime;
		this.duration = duration;
	}
}
