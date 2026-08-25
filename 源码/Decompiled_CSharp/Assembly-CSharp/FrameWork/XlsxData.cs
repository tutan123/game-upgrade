using System;
using System.Collections;
using System.Collections.Generic;

namespace FrameWork;

public class XlsxData<T, TV>
{
	public Dictionary<T, List<TV>> _dicData;

	public XlsxData(string key, IList obj)
	{
		_dicData = new Dictionary<T, List<TV>>();
		int count = obj.Count;
		for (int i = 0; i < count; i++)
		{
			T key2 = (T)obj[i].GetType().GetField(key).GetValue(obj[i]);
			if (_dicData.ContainsKey(key2))
			{
				_dicData[key2].Add((TV)obj[i]);
				continue;
			}
			_dicData.Add(key2, new List<TV> { (TV)obj[i] });
		}
	}

	public TV ByKeyGetValue(T key)
	{
		if (_dicData.TryGetValue(key, out var value))
		{
			return value[0];
		}
		return default(TV);
	}

	public List<TV> ByKeyGetValues(T key)
	{
		if (_dicData.TryGetValue(key, out var value))
		{
			return value;
		}
		return null;
	}
}
public class XlsxData<T, TK, TV>
{
	public Dictionary<T, Dictionary<TK, TV>> _dicData;

	public XlsxData(string key, string key2, IList obj)
	{
		_dicData = new Dictionary<T, Dictionary<TK, TV>>();
		int count = obj.Count;
		for (int i = 0; i < count; i++)
		{
			Type type = obj[i].GetType();
			T key3 = (T)type.GetField(key).GetValue(obj[i]);
			TK key4 = (TK)type.GetField(key2).GetValue(obj[i]);
			if (_dicData.ContainsKey(key3))
			{
				_dicData[key3].Add(key4, (TV)obj[i]);
				continue;
			}
			_dicData.Add(key3, new Dictionary<TK, TV> { 
			{
				key4,
				(TV)obj[i]
			} });
		}
	}

	public TV ByKeyGetValue(T key, TK key2)
	{
		if (_dicData.TryGetValue(key, out var value) && value.TryGetValue(key2, out var value2))
		{
			return value2;
		}
		return default(TV);
	}
}
