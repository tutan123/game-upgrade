using UnityEngine;

namespace FrameWork;

public static class MyLog
{
	public static void Log(string info)
	{
		Debug.Log(info);
	}

	public static void LogWarning(string info)
	{
		Debug.LogWarning(info);
	}

	public static void LogError(string info)
	{
		Debug.LogError(info);
	}
}
