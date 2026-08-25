using System;
using System.Collections.Generic;
using UnityEngine;

namespace FrameWork;

public class ObjectPoolAsGameObject
{
	private int _num;

	private int _currentNum;

	private GameObject _go;

	private List<GameObject> _objectPool;

	private Func<GameObject> _func;

	public ObjectPoolAsGameObject(Func<GameObject> func, int num = -1)
	{
		_func = func;
		_objectPool = new List<GameObject>();
		_currentNum = 0;
		_num = num;
	}

	public void EnQueue(GameObject go)
	{
		if (!_objectPool.Contains(go))
		{
			_objectPool.Add(go);
			go.gameObject.SetActive(value: false);
		}
		else
		{
			MyLog.LogError("添加重复的对象进对象池!!!!");
		}
	}

	public GameObject DeQueue()
	{
		if (_objectPool.Count > 0)
		{
			GameObject gameObject = _objectPool[0];
			_objectPool.RemoveAt(0);
			gameObject.SetActive(value: true);
			return gameObject;
		}
		if (_num == -1)
		{
			GameObject gameObject2 = _func();
			gameObject2.SetActive(value: true);
			return gameObject2;
		}
		if (_currentNum > _num)
		{
			return null;
		}
		GameObject gameObject3 = _func();
		gameObject3.SetActive(value: true);
		return gameObject3;
	}
}
