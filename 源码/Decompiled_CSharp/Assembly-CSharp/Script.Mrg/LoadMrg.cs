using System;
using System.Collections;
using FrameWork;
using Script.Scene;
using Script.UiTool;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Script.Mrg;

public static class LoadMrg
{
	public static void LoadAsNotyMapLoad(Scenes sceneName)
	{
		EventManager.DispatchEvent(MessageType.Game, GameMessageType.LoadScene);
		UiManager.HideAllUi("load");
		SingletonAsMono<FolderDataMrg>.Instance.Clear();
		ABMrg.ReleaseAllAssets(sceneName != Scenes.MinGameLevelSelect);
		SceneManager.LoadScene(sceneName.ToString());
	}

	public static void Load(Scenes sceneName, LoadSceneMode mode = LoadSceneMode.Single, float delay = 0.2f)
	{
		UiManager.HideAllUi("load");
		SingletonAsMono<FolderDataMrg>.Instance.Clear();
		EventManager.DispatchEvent(MessageType.Game, GameMessageType.LoadScene);
		ABMrg.ReleaseAllAssets(sceneName != Scenes.MinGameLevelSelect);
		Debug.Log($"加载场景:{sceneName}");
		MapLoadWindows mapLoadWindows;
		if (sceneName == Scenes.Map3D || (sceneName == Scenes.Game && SingletonAsMono<GameDataMrg>.Instance.GetData().is360 && !string.IsNullOrEmpty(SingletonAsMono<GameDataMrg>.Instance.GetData().event360Key)))
		{
			mapLoadWindows = UiManager.OpenUi<MapLoadWindows>();
			SingletonAsMono<FrameWork.Mono>.Instance.StartCoroutine(LoadMap());
		}
		else
		{
			SingletonAsMono<FrameWork.Mono>.Instance.StartCoroutine(LoadMap());
		}
		IEnumerator LoadMap()
		{
			yield return new WaitForSeconds(delay);
			mapLoadWindows.SetText(LanguageMrg.GetText("A1687"));
			mapLoadWindows.SetProgress(0.5f);
			AsyncOperation async2 = SceneManager.LoadSceneAsync(sceneName.ToString());
			async2.allowSceneActivation = false;
			while (async2.progress < 0.9f)
			{
				yield return null;
			}
			yield return null;
			async2.allowSceneActivation = true;
			if (!async2.isDone)
			{
				yield return null;
			}
			mapLoadWindows.SetText(LanguageMrg.GetText("A1689"));
			mapLoadWindows.SetProgress(0.9f);
			if (sceneName == Scenes.Map3D)
			{
				AsyncOperation load = ((!SingletonAsMono<GameDataMrg>.Instance.IsMorning) ? SceneManager.LoadSceneAsync("Night", LoadSceneMode.Additive) : SceneManager.LoadSceneAsync("Day", LoadSceneMode.Additive));
				while (load.progress < 0.9f)
				{
					yield return null;
				}
				yield return null;
				load.allowSceneActivation = true;
				yield return null;
				while (!load.isDone)
				{
					yield return null;
				}
				mapLoadWindows.SetText(LanguageMrg.GetText("A1689"));
				mapLoadWindows.SetProgress(0.9f);
				if ((bool)Map3DScene.Instance)
				{
					yield return Map3DScene.Instance.ShowPoint();
				}
				if ((bool)Map3DScene.Instance)
				{
					Map3DScene.Instance.ShowActor();
				}
				yield return null;
				while (Map3DScene.Instance == null)
				{
					yield return null;
				}
				if ((bool)Map3DScene.Instance)
				{
					yield return Map3DScene.Instance.ShowPoint();
				}
				yield return null;
				if (sceneName == Scenes.Map3D)
				{
					UiManager.OpenUi<MainWindows>();
				}
				yield return null;
			}
			mapLoadWindows.SetCom();
			yield return null;
			yield return new WaitForSeconds(0.1f);
			UiManager.HideUi<MapLoadWindows>();
		}
		IEnumerator LoadMap()
		{
			MapLoadWindows loadWindows = UiManager.OpenUi<MapLoadWindows>();
			yield return new WaitForSeconds(delay);
			AsyncOperation async = SceneManager.LoadSceneAsync(sceneName.ToString(), mode);
			async.allowSceneActivation = false;
			loadWindows.SetProgress(0.8f);
			while (async.progress < 0.9f)
			{
				yield return null;
			}
			yield return null;
			async.allowSceneActivation = true;
			while (!async.isDone)
			{
				yield return null;
			}
			loadWindows.SetCom();
			yield return null;
			if (sceneName == Scenes.Game && SingletonAsMono<GlobalMrg>.Instance.videoState != VideoState.Video360Scene)
			{
				while (GameScene.Instance.videoItem.IsPause())
				{
					yield return null;
				}
			}
			UiManager.HideUi<MapLoadWindows>();
		}
	}

	public static void Load(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
	{
		EventManager.DispatchEvent(MessageType.Game, GameMessageType.LoadScene);
		SceneManager.LoadScene(sceneName, mode);
		UiManager.HideAllUi("load");
	}

	public static void UnLoad(Scenes sceneName)
	{
		SceneManager.UnloadSceneAsync(sceneName.ToString());
	}

	public static void UnloadMap(string mapSceneName)
	{
		SingletonAsMono<FrameWork.Mono>.Instance.StartCoroutine(UnLoadMapEnum());
		IEnumerator UnLoadMapEnum()
		{
			UnityEngine.SceneManagement.Scene sceneByName = SceneManager.GetSceneByName(mapSceneName);
			if (sceneByName.IsValid() && sceneByName.isLoaded)
			{
				yield return SceneManager.UnloadSceneAsync(sceneByName);
				Resources.UnloadUnusedAssets();
				GC.Collect();
				Debug.Log("地图 " + mapSceneName + " 卸载完成，资源已释放");
			}
			else
			{
				Debug.LogWarning("地图 " + mapSceneName + " 未加载，无需卸载");
			}
		}
	}
}
