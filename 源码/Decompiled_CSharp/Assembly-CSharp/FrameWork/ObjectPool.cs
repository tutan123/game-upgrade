using System;
using System.Collections.Generic;

namespace FrameWork;

public class ObjectPool<T> where T : class, new()
{
	private Type _type;

	private int _num;

	private int _currentNum;

	private List<T> _objectPool;

	private Func<T> _func;

	private bool IsFun;

	public int GetSize()
	{
		return _objectPool.Count;
	}

	public ObjectPool(Func<T> func, int num = -1)
	{
		IsFun = true;
		_objectPool = new List<T>();
		_func = func;
		_type = typeof(T);
		_currentNum = 0;
		_num = num;
	}

	public ObjectPool(int num = -1)
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
			T result = _objectPool[0];
			_objectPool.RemoveAt(0);
			return result;
		}
		if (_num == -1)
		{
			if (!IsFun)
			{
				return new T();
			}
			return _func();
		}
		if (_currentNum > _num)
		{
			return null;
		}
		_currentNum++;
		if (!IsFun)
		{
			return new T();
		}
		return _func();
	}
}
