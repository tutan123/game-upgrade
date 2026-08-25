using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameWork;

public static class TimerCoroutine
{
	private static List<TimeData> _timeDatas;

	private static ObjectPool<TimeData> _objectPool;

	private static List<TimeData> _deleteList;

	static TimerCoroutine()
	{
		_timeDatas = new List<TimeData>();
		_objectPool = new ObjectPool<TimeData>();
		_deleteList = new List<TimeData>();
		SingletonAsMono<Mono>.Instance.StartCoroutine(TimeUpdate());
	}

	public static void DestroyTimer(TimeData timeData)
	{
		if (_timeDatas.Contains(timeData))
		{
			_objectPool.EnQueue(timeData);
			_timeDatas.Remove(timeData);
		}
	}

	public static TimeData IntervalCall(float time, Action call)
	{
		TimeData timeData = _objectPool.DeQueue();
		timeData.Init(isInterval: true, time, -1f, call);
		_timeDatas.Add(timeData);
		return timeData;
	}

	public static TimeData IntervalCallAsTime(float time, float intervalTime, Action call)
	{
		TimeData timeData = _objectPool.DeQueue();
		timeData.Init(isInterval: true, time, intervalTime, call);
		_timeDatas.Add(timeData);
		return timeData;
	}

	public static TimeData DelayCall(float time, Action call)
	{
		TimeData timeData = _objectPool.DeQueue();
		timeData.Init(isInterval: false, time, -1f, call);
		_timeDatas.Add(timeData);
		return timeData;
	}

	private static IEnumerator TimeUpdate()
	{
		while (true)
		{
			yield return null;
			for (int i = 0; i < _timeDatas.Count; i++)
			{
				_timeDatas[i].Update(Time.deltaTime);
			}
		}
	}

	private static void Update(float deltaTime)
	{
		for (int i = 0; i < _timeDatas.Count; i++)
		{
			_timeDatas[i].Update(deltaTime);
		}
	}
}
