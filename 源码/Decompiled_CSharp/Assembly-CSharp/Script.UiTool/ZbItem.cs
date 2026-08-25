using System.Collections.Generic;
using FrameWork;
using Script.Audio;
using Script.Mrg;
using UnityEngine;
using UnityEngine.UI;
using Xlsx;

namespace Script.UiTool;

public class ZbItem : MonoBehaviour
{
	public ZbType zbType;

	public Image addIcon;

	public Image icon;

	private void OnEnable()
	{
		Init();
		EventManager.AddListener(MessageType.Game, GameMessageType.ZbUpdate, Init);
	}

	private void OnDisable()
	{
		EventManager.RemoveListener(MessageType.Game, GameMessageType.ZbUpdate, Init);
	}

	private void Init(List<object> objects)
	{
		Init();
	}

	private void Init()
	{
		string curZbKey = SingletonAsMono<GameDataMrg>.Instance.GetCurZbKey(zbType);
		if (string.IsNullOrEmpty(curZbKey))
		{
			addIcon.SetActive(active: true);
			icon.SetActive(active: false);
			return;
		}
		addIcon.SetActive(active: false);
		icon.SetActive(active: true);
		Xlsx_Item xlsx_Item = Xlsx_Item_Query.XlsxDataAsOneKey.ByKeyGetValue(curZbKey);
		if (!string.IsNullOrEmpty(xlsx_Item.Icon))
		{
			icon.sprite = ABMrg.Load<Sprite>(xlsx_Item.Icon);
		}
	}

	public void OnClick()
	{
		string curZbKey = SingletonAsMono<GameDataMrg>.Instance.GetCurZbKey(zbType);
		if (!string.IsNullOrEmpty(curZbKey))
		{
			Xlsx_Item xlsxItem = Xlsx_Item_Query.XlsxDataAsOneKey.ByKeyGetValue(curZbKey);
			UiManager.OpenUi<ItemInfoWindows>().Init(xlsxItem, base.transform.position, isCd: false, zbType);
		}
	}

	public void OnClickRight()
	{
		if (!string.IsNullOrEmpty(SingletonAsMono<GameDataMrg>.Instance.GetCurZbKey(zbType)))
		{
			SingletonAsMono<AudioMrg>.Instance.Play(AudioDataClip.AudioData.xieXiaZhuanBei);
		}
		SingletonAsMono<GameDataMrg>.Instance.SetCurZbKey(zbType, "");
		EventManager.DispatchEvent(MessageType.Game, GameMessageType.ZbUpdate);
		EventManager.DispatchEvent(MessageType.Game, GameMessageType.UpdateProperty);
	}
}
