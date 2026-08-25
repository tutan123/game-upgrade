using System.Collections.Generic;
using FrameWork.Data;
using LitJson;
using UnityEngine;

namespace FrameWork;

public class SingletonAsMono<T> : MonoBehaviour where T : Component
{
	private static T _instance;

	public Dictionary<string, string> dic;

	public static T Instance
	{
		get
		{
			if (_instance == null)
			{
				if (Object.FindObjectOfType<T>() != null)
				{
					_instance = Object.FindObjectOfType<T>();
				}
				else
				{
					_instance = new GameObject
					{
						name = typeof(T).Name
					}.AddComponent<T>();
				}
				if (Application.isPlaying)
				{
					Object.DontDestroyOnLoad(_instance);
				}
			}
			return _instance;
		}
	}

	protected virtual void Awake()
	{
		dic = JsonMapper.ToObject<Dictionary<string, string>>(GameData.GetValue(GetDataKey(), "{}"));
	}

	protected virtual string GetDataKey()
	{
		return typeof(T).Name;
	}
}
