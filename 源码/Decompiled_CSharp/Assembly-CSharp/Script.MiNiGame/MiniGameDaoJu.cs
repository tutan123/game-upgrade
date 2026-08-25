using System.Collections.Generic;
using FrameWork;
using Script.Mrg;
using TMPro;
using UnityEngine;
using Xlsx;

namespace Script.MiNiGame;

public class MiniGameDaoJu : MonoBehaviour
{
	public Xlsx_MiNiGameItem_Key xlsxMiniGameItemKey;

	public TMP_Text count;

	public TMP_Text price;

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
		price.text = "x" + _xlsxMiniGameItem.Price;
		int property = SingletonAsMono<GameDataMrg>.Instance.GetProperty(_xlsxMiniGameItem.Key + "_Get", "Nor", 0L);
		if (count != null)
		{
			count.text = (_xlsxMiniGameItem.MaxGet - property).ToString() ?? "";
		}
	}

	public void Click()
	{
		int value = _xlsxMiniGameItem.Price;
		int property = SingletonAsMono<GameDataMrg>.Instance.GetProperty(_xlsxMiniGameItem.Key + "_Get", "Nor", 0L);
		if (_xlsxMiniGameItem.MaxGet != -1 && property >= _xlsxMiniGameItem.MaxGet)
		{
			UiManager.ShowTips(LanguageMrg.GetText("A1297"));
		}
		else if (UiManager.GetUi<LevelItemSelectWindows>().IsHasTiLi(value))
		{
			SingletonAsMono<GameDataMrg>.Instance.AddProperty(_xlsxMiniGameItem.Key, 1L);
			SingletonAsMono<GameDataMrg>.Instance.AddProperty(_xlsxMiniGameItem.Key + "_Get", 1L);
			UiManager.GetUi<LevelItemSelectWindows>().AddZhiLi(value);
			Init();
			UiManager.GetUi<LevelItemSelectWindows>().InitZhiLi();
		}
		else
		{
			UiManager.ShowTips(LanguageMrg.GetText("A1296"));
		}
	}
}
