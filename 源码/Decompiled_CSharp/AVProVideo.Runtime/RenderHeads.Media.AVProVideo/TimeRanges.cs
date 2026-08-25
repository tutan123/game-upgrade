using System;
using System.Collections;

namespace RenderHeads.Media.AVProVideo;

public class TimeRanges : IEnumerable
{
	internal TimeRange[] _ranges = new TimeRange[0];

	internal double _minTime;

	internal double _maxTime;

	public TimeRange this[int index] => _ranges[index];

	public int Count => _ranges.Length;

	public double MinTime => _minTime;

	public double MaxTime => _maxTime;

	public double Duration => _maxTime - _minTime;

	internal TimeRanges()
	{
	}

	public IEnumerator GetEnumerator()
	{
		return _ranges.GetEnumerator();
	}

	public override string ToString()
	{
		return $"TimeRanges: {{ MinTime: {MinTime}, MaxTime: {MaxTime}, Duration: {Duration}, Count: {Count} }}";
	}

	internal TimeRanges(TimeRange[] ranges)
	{
		_ranges = ranges;
		CalculateRange();
	}

	internal void CalculateRange()
	{
		_minTime = (_maxTime = 0.0);
		if (_ranges != null && _ranges.Length != 0)
		{
			double num = 0.0;
			double num2 = double.MaxValue;
			for (int i = 0; i < _ranges.Length; i++)
			{
				num2 = Math.Min(num2, _ranges[i].startTime);
				num = Math.Max(num, _ranges[i].startTime + _ranges[i].duration);
			}
			_minTime = num2;
			_maxTime = num;
		}
	}
}
