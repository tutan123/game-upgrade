using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace FrameWork;

public static class ABMrg
{
	private static List<AsyncOperationHandle> _handles = new List<AsyncOperationHandle>();

	public static void LoadAsync<T>(string name, Action<T> handle)
	{
		AsyncOperationHandle<T> asyncOperationHandle = Addressables.LoadAssetAsync<T>(name);
		asyncOperationHandle.Completed += delegate(AsyncOperationHandle<T> h)
		{
			handle?.Invoke(h.Result);
		};
	}

	public static T Load<T>(string name, bool isAutoRelease = true, bool isAdd = true)
	{
		try
		{
			AsyncOperationHandle<T> asyncOperationHandle = Addressables.LoadAssetAsync<T>(name);
			T result = asyncOperationHandle.WaitForCompletion();
			if (isAdd)
			{
				_handles.Add(asyncOperationHandle);
			}
			return result;
		}
		catch (Exception ex)
		{
			MyLog.LogError(ex.Message);
			return default(T);
		}
	}

	public static void Release(GameObject obj)
	{
		Addressables.ReleaseInstance(obj);
	}

	public static void ReleaseAllAssets(bool isClearAA = true)
	{
		if (isClearAA)
		{
			foreach (AsyncOperationHandle handle in _handles)
			{
				if (handle.IsValid())
				{
					Addressables.Release(handle);
				}
			}
			_handles.Clear();
		}
		Resources.UnloadUnusedAssets();
		GC.Collect();
	}
}
