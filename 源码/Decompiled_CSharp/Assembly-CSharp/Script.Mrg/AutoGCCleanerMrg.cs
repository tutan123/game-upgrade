using System;
using FrameWork;
using UnityEngine;

namespace Script.Mrg;

public class AutoGCCleanerMrg : SingletonAsMono<AutoGCCleanerMrg>
{
	[Header("GC间隔 秒")]
	private float gcInterval = 1f;

	[Header("仅后台/闲置执行")]
	private bool onlyIdle = true;

	[Header("是否卸载无用资源")]
	private bool unloadUnusedAsset;

	private float _timer;

	private void Update()
	{
		_timer += Time.unscaledDeltaTime;
		if (_timer >= gcInterval)
		{
			_timer = 0f;
			DoLightGC();
		}
	}

	private void DoLightGC()
	{
		if (!onlyIdle || !(Time.timeScale > 0.1f))
		{
			GC.Collect(0, GCCollectionMode.Optimized);
			GC.WaitForPendingFinalizers();
			if (unloadUnusedAsset)
			{
				Resources.UnloadUnusedAssets();
			}
		}
	}

	public static void QuickLightGC()
	{
		GC.Collect(0, GCCollectionMode.Optimized);
		GC.WaitForPendingFinalizers();
	}
}
