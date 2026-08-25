using System;
using System.Collections;
using FrameWork;
using FrameWork.Data;
using Script.Audio;
using Script.Mrg;
using Steamworks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace Script.Scene;

public class LoadScene : MonoBehaviour
{
	public Slider slider;

	public TMP_Text sliderText;

	public TMP_Text tipsText;

	private float _progress;

	private void Start()
	{
		tipsText.text = FrameWork.Tool.GetLoadTips();
		StartCoroutine(InitData());
	}

	private string GetTimeData()
	{
		return DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
	}

	private void SetText(string text)
	{
		sliderText.text = text;
	}

	private void SetProgress(float progress)
	{
		_progress = progress;
	}

	public void SetCom()
	{
		SetText(LanguageMrg.GetText("A1688"));
		SetProgress(1f);
		slider.value = 1f;
	}

	private void Update()
	{
		if (slider.value < _progress)
		{
			slider.value += 0.2f * Time.deltaTime;
		}
	}

	private IEnumerator InitData()
	{
		Debug.Log("开始加载load界面:" + GetTimeData());
		yield return null;
		yield return new WaitForSeconds(0.2f);
		Debug.Log("开始初始化存档:" + GetTimeData());
		GameData.Init();
		yield return null;
		Debug.Log("生成一些管理类:" + GetTimeData());
		Debug.Log("初始化InfoTipsMrg:" + GetTimeData());
		_ = SingletonAsMono<InfoTipsMrg>.Instance;
		yield return null;
		Debug.Log("初始化EventContentMrg:" + GetTimeData());
		yield return SingletonAsMono<EventContentMrg>.Instance.LoadEventContent();
		yield return null;
		Debug.Log("初始化GameDataMrg:" + GetTimeData());
		_ = SingletonAsMono<GameDataMrg>.Instance;
		yield return null;
		Debug.Log("初始化MouseAutoHideMrg:" + GetTimeData());
		_ = SingletonAsMono<MouseAutoHideMrg>.Instance;
		yield return null;
		Debug.Log("初始化显示方式和画质:" + GetTimeData());
		GameData.SetDisPlay();
		GameData.SetQualityApply();
		yield return null;
		SetText(LanguageMrg.GetText("A1686"));
		SetProgress(0.4f);
		yield return null;
		Debug.Log("SteamSdk初始化:" + GetTimeData());
		if (SteamAPI.Init())
		{
			Debug.Log("SteamSdk初始化成功:" + GetTimeData());
		}
		else
		{
			Debug.Log("SteamSdk初始化失败:" + GetTimeData());
		}
		yield return null;
		SetText(LanguageMrg.GetText("A1685"));
		SetProgress(0.6f);
		Debug.Log("预加载ui:" + GetTimeData());
		yield return PreheatUIReal(delegate(LoadSaveWindows w)
		{
			w.InitType(isLoad: true, null, isInit: true);
		});
		yield return null;
		yield return PreheatUIReal<SettingWindows>();
		yield return null;
		yield return PreheatUIReal<MapLoadWindows>();
		yield return null;
		Canvas.ForceUpdateCanvases();
		yield return null;
		SetText(LanguageMrg.GetText("A2329"));
		SetProgress(0.8f);
		yield return null;
		Debug.Log("预加载AudioData:" + GetTimeData());
		yield return Addressables.LoadAssetAsync<AudioData>("AudioData");
		yield return null;
		Debug.Log("完成:" + GetTimeData());
		SetCom();
		GameData.IsLoad = true;
		yield return new WaitForSeconds(0.2f);
		if (GameData.IsFistJoinGame)
		{
			LoadMrg.LoadAsNotyMapLoad(Scenes.Game);
			GameData.IsFistJoinGame = false;
			GameData.Save();
		}
		else
		{
			LoadMrg.LoadAsNotyMapLoad(Scenes.Start);
		}
		yield return null;
	}

	private IEnumerator PreheatUIReal<T>(Action<T> init = null) where T : UiActor
	{
		T val = UiManager.OpenUi<T>();
		init?.Invoke(val);
		CanvasGroup cg = val.transform.GetComponent<CanvasGroup>();
		if (cg == null)
		{
			cg = val.transform.AddComponent<CanvasGroup>();
		}
		cg.alpha = 0f;
		cg.blocksRaycasts = false;
		cg.interactable = false;
		cg.ignoreParentGroups = true;
		yield return null;
		Canvas.ForceUpdateCanvases();
		UiManager.HideUi<T>();
		cg.alpha = 1f;
		cg.blocksRaycasts = true;
		cg.interactable = true;
		yield return null;
	}
}
