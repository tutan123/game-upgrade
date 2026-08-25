using System;
using System.Collections;
using UnityEngine;

namespace FrameWork;

public class Mono : SingletonAsMono<Mono>
{
	public void Frame(Action action)
	{
		StartCoroutine(_Frame(action));
	}

	private IEnumerator _Frame(Action action)
	{
		yield return null;
		action?.Invoke();
		if (!Application.isPlaying)
		{
			UnityEngine.Object.DestroyImmediate(base.gameObject);
		}
	}

	public void Wait(float time, Action action)
	{
		StartCoroutine(_Wait(time, action));
	}

	private IEnumerator _Wait(float time, Action action)
	{
		yield return new WaitForSeconds(time);
		action?.Invoke();
	}
}
