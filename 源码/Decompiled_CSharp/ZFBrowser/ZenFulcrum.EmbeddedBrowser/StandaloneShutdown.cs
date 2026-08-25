using UnityEngine;

namespace ZenFulcrum.EmbeddedBrowser;

internal class StandaloneShutdown : MonoBehaviour
{
	public static void Create()
	{
		GameObject obj = new GameObject("ZFB Shutdown");
		obj.AddComponent<StandaloneShutdown>();
		Object.DontDestroyOnLoad(obj);
	}

	public void OnApplicationQuit()
	{
		BrowserNative.UnloadNative();
	}
}
