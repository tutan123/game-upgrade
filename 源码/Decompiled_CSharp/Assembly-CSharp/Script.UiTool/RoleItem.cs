using System.Collections.Generic;
using FrameWork;
using Script.Mrg;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Xlsx;

namespace Script.UiTool;

public class RoleItem : MonoBehaviour
{
	public GameObject bg;

	public GameObject zb;

	public Image icon;

	public TMP_Text count;

	public GameObject red;

	private Xlsx_Item _xlsxItem;

	public void Init(Xlsx_Item xlsxItem)
	{
		_xlsxItem = xlsxItem;
		Init();
	}

	private void Init()
	{
		if (_xlsxItem == null)
		{
			icon.SetActive(active: false);
			zb.SetActive(value: false);
			bg.SetActive(value: true);
			red.SetActiveAsCheck(active: false);
			return;
		}
		icon.SetActive(active: true);
		ZbType itemType = (ZbType)_xlsxItem.ItemType;
		zb.SetActive(value: false);
		bg.SetActive(value: false);
		int property = SingletonAsMono<GameDataMrg>.Instance.GetProperty(_xlsxItem.Key, "Item", 0L);
		count.SetActive(property > 1);
		count.text = property.ToString().FormatStringNumber();
		string curZbKey = SingletonAsMono<GameDataMrg>.Instance.GetCurZbKey(itemType);
		List<string> list = new List<string>();
		red.SetActiveAsCheck(SingletonAsMono<GameDataMrg>.Instance.GetItemGetIsShowRed(_xlsxItem.Key));
		if (!string.IsNullOrEmpty(_xlsxItem.Icon))
		{
			icon.sprite = ABMrg.Load<Sprite>(_xlsxItem.Icon);
		}
		if (!string.IsNullOrEmpty(curZbKey))
		{
			list.Add(curZbKey);
		}
		if (itemType == ZbType.PeiShi)
		{
			if (!string.IsNullOrEmpty(SingletonAsMono<GameDataMrg>.Instance.GetCurZbKey(ZbType.PeiShi2)))
			{
				list.Add(SingletonAsMono<GameDataMrg>.Instance.GetCurZbKey(ZbType.PeiShi2));
			}
			if (!string.IsNullOrEmpty(SingletonAsMono<GameDataMrg>.Instance.GetCurZbKey(ZbType.PeiShi3)))
			{
				list.Add(SingletonAsMono<GameDataMrg>.Instance.GetCurZbKey(ZbType.PeiShi3));
			}
		}
		if (list.Count == 0)
		{
			bg.SetActive(value: true);
		}
		else if (list.Contains(_xlsxItem.Key))
		{
			zb.SetActive(value: true);
		}
		else
		{
			bg.SetActive(value: true);
		}
	}

	public void Click()
	{
		if (_xlsxItem != null)
		{
			if (SingletonAsMono<GameDataMrg>.Instance.GetItemGetIsShowRed(_xlsxItem.Key))
			{
				SingletonAsMono<GameDataMrg>.Instance.SetItemShowRed(_xlsxItem.Key, value: false);
				EventManager.DispatchEvent(MessageType.Game, GameMessageType.ZbUpdate);
			}
			UiManager.OpenUi<ItemInfoWindows>().Init(_xlsxItem, base.transform.position);
		}
	}

	public void UseItem()
	{
		if (_xlsxItem != null)
		{
			if (SingletonAsMono<GameDataMrg>.Instance.GetItemGetIsShowRed(_xlsxItem.Key))
			{
				SingletonAsMono<GameDataMrg>.Instance.SetItemShowRed(_xlsxItem.Key, value: false);
				EventManager.DispatchEvent(MessageType.Game, GameMessageType.ZbUpdate);
			}
			FrameWork.Tool.ClickZb(_xlsxItem);
		}
	}
}
