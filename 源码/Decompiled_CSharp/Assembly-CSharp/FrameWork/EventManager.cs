using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace FrameWork;

public static class EventManager
{
	private static ConcurrentDictionary<int, ConcurrentDictionary<int, Action<List<object>>>> _listeners;

	private static ObjectPool<List<object>> _objectPool;

	public static void Init()
	{
		_listeners.Clear();
	}

	static EventManager()
	{
		_listeners = new ConcurrentDictionary<int, ConcurrentDictionary<int, Action<List<object>>>>();
		_objectPool = new ObjectPool<List<object>>();
	}

	public static void AddListener(object evtType, object evt, Action<List<object>> listener)
	{
		AddListener((int)evtType, (int)evt, listener);
	}

	public static void AddListener(int evtType, int evt, Action<List<object>> listener)
	{
		if (_listeners.ContainsKey(evtType))
		{
			if (!_listeners[evtType].ContainsKey(evt))
			{
				_listeners[evtType].TryAdd(evt, listener);
				return;
			}
			ConcurrentDictionary<int, Action<List<object>>> concurrentDictionary = _listeners[evtType];
			concurrentDictionary[evt] = (Action<List<object>>)Delegate.Combine(concurrentDictionary[evt], listener);
		}
		else
		{
			ConcurrentDictionary<int, Action<List<object>>> concurrentDictionary2 = new ConcurrentDictionary<int, Action<List<object>>>();
			concurrentDictionary2.TryAdd(evt, listener);
			_listeners.TryAdd(evtType, concurrentDictionary2);
		}
	}

	public static void DispatchEvent(object evtType, object evt, List<object> data = null)
	{
		DispatchEvent((int)evtType, (int)evt, data);
	}

	public static void DispatchEvent(int evtType, int evt, List<object> data = null)
	{
		if (_listeners.ContainsKey(evtType) && _listeners[evtType].ContainsKey(evt))
		{
			_listeners[evtType][evt]?.Invoke(data);
			if (data != null)
			{
				_objectPool.EnQueue(data);
			}
		}
	}

	public static void RemoveListener(object evtType, object evt, Action<List<object>> listener)
	{
		RemoveListener((int)evtType, (int)evt, listener);
	}

	public static void RemoveListener(int evtType, int evt, Action<List<object>> listener)
	{
		if (_listeners.ContainsKey(evtType) && _listeners[evtType].ContainsKey(evt))
		{
			ConcurrentDictionary<int, Action<List<object>>> concurrentDictionary = _listeners[evtType];
			concurrentDictionary[evt] = (Action<List<object>>)Delegate.Remove(concurrentDictionary[evt], listener);
		}
	}

	public static List<object> GetEventMsg()
	{
		List<object> list = _objectPool.DeQueue();
		list.Clear();
		return list;
	}
}
