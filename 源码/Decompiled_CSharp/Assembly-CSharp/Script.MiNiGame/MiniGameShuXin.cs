using System.Collections.Generic;
using FrameWork;
using Script.Mrg;
using TMPro;
using UnityEngine;
using Xlsx;

namespace Script.MiNiGame;

public class MiniGameShuXin : MonoBehaviour
{
	public Transform starParent;

	public TMP_Text priceText;

	public TMP_Text addText;

	public Xlsx_MiNiGameItem_Key xlsxMiniGameItemKey;

	private Xlsx_MiNiGameItem _xlsxMiniGameItem;

	private void OnEnable()
	{
		_xlsxMiniGameItem = Xlsx_MiNiGameItem_Query.XlsxDataAsOneKey.ByKeyGetValue(xlsxMiniGameItemKey.ToString());
		Init();
		EventManager.AddListener(MessageType.Game, GameMessageType.ResetMiniGameItem, ResetInit);
	}

	private void OnDisable()
	{
		EventManager.RemoveListener(MessageType.Game, GameMessageType.ResetMiniGameItem, ResetInit);
	}

	private void ResetInit(List<object> objects)
	{
		Init();
	}

	private void Init()
	{
		int property = SingletonAsMono<GameDataMrg>.Instance.GetProperty(_xlsxMiniGameItem.Key, "Nor", 0L);
		priceText.text = "x" + _xlsxMiniGameItem.Price;
		addText.text = $"(+{_xlsxMiniGameItem.Add * (float)property * 100f}%)";
		for (int i = 0; i < starParent.childCount; i++)
		{
			starParent.GetChild(i).GetChild(0).SetActive(i < property);
		}
	}

	public void Click()
	{
		int price = _xlsxMiniGameItem.Price;
		int property = SingletonAsMono<GameDataMrg>.Instance.GetProperty(_xlsxMiniGameItem.Key + "_Get", "Nor", 0L);
		if (_xlsxMiniGameItem.MaxGet != -1 && property >= _xlsxMiniGameItem.MaxGet)
		{
			UiManager.ShowTips(LanguageMrg.GetText("A1298"));
		}
		else if (UiManager.GetUi<LevelItemSelectWindows>().IsHasTiLi(price))
		{
			SingletonAsMono<GameDataMrg>.Instance.AddProperty(_xlsxMiniGameItem.Key, 1L);
			SingletonAsMono<GameDataMrg>.Instance.AddProperty(_xlsxMiniGameItem.Key + "_Get", 1L);
			UiManager.GetUi<LevelItemSelectWindows>().AddZhiLi(price);
			Init();
			UiManager.GetUi<LevelItemSelectWindows>().InitZhiLi();
		}
		else
		{
			UiManager.ShowTips(LanguageMrg.GetText("A1296"));
		}
	}
}
