using System;
using System.Collections.Generic;
using FrameWork;
using Script.Mrg;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Xlsx;

namespace Script.MiNiGame;

public class LevelSelectItem : MonoBehaviour
{
	public string _xlsxMiNiGameLevelKey;

	public Image icon;

	public TMP_Text title;

	public TMP_Text kpiText;

	public GameObject fistGo;

	public TMP_Text fistText;

	public GameObject maskGo;

	public GameObject go;

	private Xlsx_MiNiGameLevel _xlsxMiNiGameLevel;

	public void Init(string xlsxMiNiGameLevelKey)
	{
		_xlsxMiNiGameLevelKey = xlsxMiNiGameLevelKey;
		_xlsxMiNiGameLevel = Xlsx_MiNiGameLevel_Query.XlsxDataAsOneKey.ByKeyGetValue(_xlsxMiNiGameLevelKey.ToString());
		bool flag = _xlsxMiNiGameLevelKey == "A1" || SingletonAsMono<GameDataMrg>.Instance.IsPassMiniGame((Enum.Parse<Xlsx_MiNiGameLevel_Key>(_xlsxMiNiGameLevel.Key) - 1).ToString());
		if (xlsxMiNiGameLevelKey == "A5")
		{
			flag = SingletonAsMono<GameDataMrg>.Instance.CurRound >= 17;
		}
		icon.SetActive(active: true);
		go.SetActiveAsCheck(SingletonAsMono<GameDataMrg>.Instance.IsPassMiniGame(_xlsxMiNiGameLevel.Key));
		if (flag)
		{
			icon.sprite = ABMrg.Load<Sprite>(_xlsxMiNiGameLevel.Icon);
			title.text = LanguageMrg.GetText(_xlsxMiNiGameLevel.Title);
			kpiText.text = Enum.Parse<PropertyTypeValue>(_xlsxMiNiGameLevel.PassReward[0]).GetName() + "+" + _xlsxMiNiGameLevel.PassReward[1];
			maskGo.SetActiveAsCheck(active: false);
			string[] fistReward = _xlsxMiNiGameLevel.FistReward;
			long[] fistRewardCount = _xlsxMiNiGameLevel.FistRewardCount;
			string text = "";
			for (int i = 0; i < fistReward.Length; i++)
			{
				text = text + fistReward[i].ToEnum<PropertyTypeValue>().GetName() + "+" + fistRewardCount[i] + " ";
			}
			fistText.text = text;
		}
		else
		{
			fistText.text = "？？？？";
			title.text = "？？？？";
			kpiText.text = "？？？？";
			maskGo.SetActiveAsCheck(active: true);
		}
		fistGo.SetActiveAsCheck(!SingletonAsMono<GameDataMrg>.Instance.IsPassMiniGame(_xlsxMiNiGameLevel.Key) && flag);
		if (fistGo.activeSelf && string.IsNullOrEmpty(fistText.text))
		{
			fistGo.SetActiveAsCheck(active: false);
		}
	}

	public void Click()
	{
		bool flag = _xlsxMiNiGameLevelKey == "A1" || SingletonAsMono<GameDataMrg>.Instance.IsPassMiniGame((Enum.Parse<Xlsx_MiNiGameLevel_Key>(_xlsxMiNiGameLevel.Key) - 1).ToString());
		if (_xlsxMiNiGameLevelKey == "A5")
		{
			flag = SingletonAsMono<GameDataMrg>.Instance.CurRound >= 17;
		}
		if (flag)
		{
			SingletonAsMono<GameDataMrg>.Instance.xlsxMiNiGameLevelKey = _xlsxMiNiGameLevel.Key;
			UiManager.HideUi<LevelSelectWindows>();
			UiManager.OpenUi<LevelItemSelectWindows>();
		}
	}

	public void Skip()
	{
		SingletonAsMono<GameDataMrg>.Instance.xlsxMiNiGameLevelKey = _xlsxMiNiGameLevel.Key;
		GameOver();
	}

	private void GameOver()
	{
		if (!SingletonAsMono<GameDataMrg>.Instance.IsPassMiniGame(_xlsxMiNiGameLevel.Key))
		{
			string[] fistReward = _xlsxMiNiGameLevel.FistReward;
			SingletonAsMono<GameDataMrg>.Instance.AddProperty(fistReward[0], int.Parse(fistReward[1]), "Property");
		}
		SingletonAsMono<GameDataMrg>.Instance.PassMiniGame(_xlsxMiNiGameLevel.Key);
		string[] passReward = _xlsxMiNiGameLevel.PassReward;
		SingletonAsMono<GameDataMrg>.Instance.AddProperty(passReward[0], int.Parse(passReward[1]), "Property");
		string text = string.Format(LanguageMrg.GetText("A5488"), SingletonAsMono<GameDataMrg>.Instance.GetProperty("KPI", "Property", 0L)) + "(+" + _xlsxMiNiGameLevel.PassReward[1] + ")";
		VideoNode videoNode = SingletonAsMono<GlobalMrg>.Instance.videoNode;
		if (videoNode != null && videoNode.isToNextGroup)
		{
			SingletonAsMono<GameDataMrg>.Instance.CurEventKey = ((VideoGraph)videoNode.graph).xlsxEventKey.ToString();
			SingletonAsMono<GameDataMrg>.Instance.CurVideoId = videoNode.uniqueID;
			UiManager.OpenUi<LevelSucceedWindows>().Init(text, delegate
			{
				LoadMrg.Load(Scenes.Game);
			});
		}
		else if (videoNode != null && videoNode.isPlayerEndPlayerNext)
		{
			VideoNode videoNode2 = null;
			if (videoNode.isPlayerEndPlayerNextRandom)
			{
				List<VideoNode> allVideoNodesOutNor = videoNode.GetAllVideoNodesOutNor("nextVideoNode");
				videoNode2 = allVideoNodesOutNor[UnityEngine.Random.Range(0, allVideoNodesOutNor.Count)];
			}
			else if (videoNode.isPlayerEndPlayerNextSequence)
			{
				List<VideoNode> allVideoNodesOutNor2 = videoNode.GetAllVideoNodesOutNor("nextVideoNode");
				VideoNode videoNode3 = videoNode;
				int num = SingletonAsMono<GameDataMrg>.Instance.GetProperty(videoNode3.uniqueID, "Index", 0L);
				if (num >= allVideoNodesOutNor2.Count)
				{
					num = allVideoNodesOutNor2.Count - 1;
				}
				videoNode2 = allVideoNodesOutNor2[num];
				SingletonAsMono<GameDataMrg>.Instance.AddProperty(videoNode3.uniqueID, 1L, "Index");
			}
			else
			{
				videoNode2 = videoNode.GetAllVideoNodesOutNor("nextVideoNode")[0];
			}
			SingletonAsMono<GameDataMrg>.Instance.CurEventKey = ((VideoGraph)videoNode2.graph).xlsxEventKey.ToString();
			SingletonAsMono<GameDataMrg>.Instance.CurVideoId = videoNode2.uniqueID;
			if (videoNode2.isToMap)
			{
				LoadMrg.Load(Scenes.Map3D);
			}
			else
			{
				LoadMrg.Load(Scenes.Game);
			}
		}
		else
		{
			UiManager.OpenUi<LevelSucceedWindows>().Init(text, delegate
			{
				LoadMrg.Load(Scenes.Map3D);
			});
		}
	}
}
