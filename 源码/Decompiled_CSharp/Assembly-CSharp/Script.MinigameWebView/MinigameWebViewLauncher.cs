using System;
using System.IO;
using System.Threading.Tasks;
using DG.Tweening;
using FrameWork;
using Script.Mrg;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ZenFulcrum.EmbeddedBrowser;

namespace Script.MinigameWebView;

public class MinigameWebViewLauncher : MonoBehaviour
{
	private string gameFolder = "Pinball";

	private string indexFile = "index.html";

	public Browser browser;

	public Image mask;

	private Tweener _tweener;

	private void Awake()
	{
		browser.onLoad += LoadSuccess;
		RegisterFunctionAll();
	}

	public async void Init(string folder)
	{
		await OpenGame(folder);
	}

	private void OnDestroy()
	{
	}

	public async Task OpenGame(string folder)
	{
		gameFolder = folder;
		EnsureEventSystem();
		_tweener?.Kill();
		mask.SetActive(active: true);
		mask.color = Color.black;
		try
		{
			string html = File.ReadAllText(GetGamePath());
			browser.LoadHTML(html);
		}
		catch (Exception ex)
		{
			Debug.LogError(ex.Message);
		}
	}

	private void LoadSuccess(JSONNode jsonNode)
	{
		_tweener = mask.DOColor(new Color(0f, 0f, 0f, 0f), 0.2f);
		Tweener tweener = _tweener;
		tweener.onComplete = (TweenCallback)Delegate.Combine(tweener.onComplete, (TweenCallback)delegate
		{
			mask.SetActive(active: false);
		});
	}

	public async void OpenPinball()
	{
		await OpenGame("Pinball");
	}

	public string GetGamePath()
	{
		string text = Uri.EscapeDataString(gameFolder.Trim('/'));
		string text2 = Uri.EscapeDataString(indexFile.Trim('/'));
		return Application.streamingAssetsPath + "/MiniGame/" + text + "/" + text2;
	}

	private void EnsureEventSystem()
	{
		if (!(EventSystem.current != null))
		{
			GameObject obj = new GameObject("EventSystem");
			obj.AddComponent<EventSystem>();
			obj.AddComponent<StandaloneInputModule>();
		}
	}

	private void Update()
	{
		CallFunction();
	}

	private void CallFunction()
	{
		if (browser.IsLoaded)
		{
			browser.CallFunction("setGameMoney", SingletonAsMono<GameDataMrg>.Instance.GetProperty("Money", "Property", 0L));
		}
	}

	private void RegisterFunctionAll()
	{
		browser.RegisterFunction("OnMoneyChanged", delegate(JSONNode args)
		{
			if (args.Count > 0)
			{
				SingletonAsMono<GameDataMrg>.Instance.AddProperty("Money", args[0].AsJSON.ToInt(), "Property");
			}
		});
		browser.RegisterFunction("OnGameEnded", delegate
		{
		});
	}
}
