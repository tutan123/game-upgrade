using System.Collections.Generic;
using FrameWork;
using FrameWork.Data;
using Script.Mrg;
using Script.Scene;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Script.UiTool;

public class SaveItem : MonoBehaviour
{
	public TMP_Text index;

	public TMP_Text saveName;

	public TMP_Text bout;

	public TMP_Text time;

	public Image newImg;

	private SavedData _savedData;

	public GameObject bgGo;

	public GameObject maskGo;

	private bool _isLoad;

	public void Init(SavedData savedData, int xh, bool isLoad = true, bool isNew = false)
	{
		if (xh + 1 < 10)
		{
			index.text = "0" + (xh + 1);
		}
		else
		{
			index.text = (xh + 1).ToString() ?? "";
		}
		_savedData = savedData;
		saveName.text = savedData.SavedName;
		string text = (savedData.Data.GetValueOrDefault("IsMorning", true.ToString()).ToBool() ? LanguageMrg.GetText("A5193") : LanguageMrg.GetText("A5194"));
		bout.text = string.Format(LanguageMrg.GetText("A433"), savedData.Round) + text;
		time.text = FrameWork.Tool.GetTime(savedData.Time.ToLong());
		newImg.SetActive(isNew);
		_isLoad = isLoad;
		bgGo.SetActiveAsCheck(savedData.Data.Count > 0);
		maskGo.SetActiveAsCheck(savedData.Data.Count == 0);
	}

	public void Click()
	{
		if (_isLoad)
		{
			if (_savedData.Data.Count == 0)
			{
				return;
			}
			UiManager.OpenUi<SureWindows>().Init(LanguageMrg.GetText("A1597"), delegate
			{
				GameData.CurKey = _savedData.Key;
				SingletonAsMono<GameDataMrg>.Instance.LoadSaveData();
				SingletonAsMono<GameDataMrg>.Instance.SetIsNew(isNew: false);
				if (!SingletonAsMono<GameDataMrg>.Instance.GetData().is360)
				{
					LoadMrg.Load(Scenes.Map3D);
				}
				else
				{
					SingletonAsMono<GameDataMrg>.Instance.CurEventKey = SingletonAsMono<GameDataMrg>.Instance.GetData().event360Key;
					SingletonAsMono<GameDataMrg>.Instance.CurVideoId = "";
					LoadMrg.Load(Scenes.Game);
				}
				if (UiManager.IsOpenUi<LoadSaveWindows>())
				{
					UiManager.GetUi<LoadSaveWindows>().LoadClose();
				}
				if (UiManager.IsOpenUi<SaveWindows>())
				{
					UiManager.GetUi<SaveWindows>().LoadClose();
				}
			});
		}
		else
		{
			if (SingletonAsMono<GameDataMrg>.Instance.GetAutoSaveKeys().Contains(_savedData.Key))
			{
				return;
			}
			UiManager.OpenUi<SureWindows>().Init(LanguageMrg.GetText("A1594"), delegate
			{
				UiManager.OpenUi<InputWindows>().Init(LanguageMrg.GetText("A1595"), delegate(string s)
				{
					if (SingletonAsMono<GlobalMrg>.Instance.videoState == VideoState.Map && (bool)Map3DScene.Instance)
					{
						SingletonAsMono<GameDataMrg>.Instance.MapLoc = Map3DScene.Instance.player.transform.position;
					}
					SingletonAsMono<GameDataMrg>.Instance.CoverSave(_savedData.Key, s);
					if (UiManager.IsOpenUi<SaveWindows>())
					{
						UiManager.GetUi<SaveWindows>().Init();
					}
					if (UiManager.IsOpenUi<LoadSaveWindows>())
					{
						UiManager.GetUi<LoadSaveWindows>().Init();
					}
				}, isCanClose: true, _savedData.SavedName);
			});
		}
	}

	public void ClickDelete()
	{
		UiManager.OpenUi<SureWindows>().Init(LanguageMrg.GetText("A1598"), delegate
		{
			SingletonAsMono<GameDataMrg>.Instance.RemoveSave(_savedData.Key);
			if (UiManager.IsOpenUi<SaveWindows>())
			{
				UiManager.GetUi<SaveWindows>().Init();
			}
			if (UiManager.IsOpenUi<LoadSaveWindows>())
			{
				UiManager.GetUi<LoadSaveWindows>().Init();
			}
		});
	}
}
