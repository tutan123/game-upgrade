using System;
using System.Collections.Generic;
using UnityEngine;

namespace FrameWork;

public class ObjectPoolAsComponent<T> where T : Component
{
	private Type _type;

	private int _num;

	private int _currentNum;

	private List<T> _objectPool;

	private Func<T> _func;

	private bool IsFun;

	public ObjectPoolAsComponent(Func<T> func, int num = -1)
	{
		_func = func;
		_objectPool = new List<T>();
		_type = typeof(T);
		_currentNum = 0;
		_num = num;
	}

	public ObjectPoolAsComponent(int num = -1)
	{
		IsFun = false;
		_objectPool = new List<T>();
		_type = typeof(T);
		_currentNum = 0;
		_num = num;
	}

	public void EnQueue(T t)
	{
		if (!_objectPool.Contains(t))
		{
			_objectPool.Add(t);
			t.gameObject.SetActive(value: false);
		}
		else
		{
			MyLog.LogError("添加重复的对象进对象池!!!!");
		}
	}

	public T DeQueue()
	{
		if (_objectPool.Count > 0)
		{
			T val = _objectPool[0];
			_objectPool.RemoveAt(0);
			val.gameObject.SetActive(value: true);
			return val;
		}
		if (_num == -1)
		{
			if (IsFun)
			{
				Func<T> func = _func;
				if (func == null)
				{
					return null;
				}
				return func();
			}
			GameObject gameObject = new GameObject(_type.Name);
			gameObject.SetActive(value: true);
			return gameObject.AddComponent(_type) as T;
		}
		if (_currentNum > _num)
		{
			return null;
		}
		_currentNum++;
		if (IsFun)
		{
			Func<T> func2 = _func;
			if (func2 == null)
			{
				return null;
			}
			return func2();
		}
		GameObject gameObject2 = new GameObject(_type.Name);
		gameObject2.SetActive(value: true);
		return gameObject2.AddComponent(_type) as T;
	}
}
