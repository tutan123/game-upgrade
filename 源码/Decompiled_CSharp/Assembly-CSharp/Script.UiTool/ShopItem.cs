using System;
using FrameWork;
using Script.Mrg;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Xlsx;

namespace Script.UiTool;

public class ShopItem : MonoBehaviour
{
	public Image icon;

	public TMP_Text iconName;

	public TMP_Text iconCoin;

	public GameObject showPos;

	public GameObject sumGetBtn;

	private Xlsx_Item _xlsxItem;

	public void Init(Xlsx_Item xlsxItem)
	{
		_xlsxItem = xlsxItem;
		icon.SetActive(active: true);
		if (!string.IsNullOrEmpty(xlsxItem.Icon))
		{
			icon.sprite = ABMrg.Load<Sprite>(xlsxItem.Icon);
		}
		else
		{
			icon.SetActive(active: false);
		}
		sumGetBtn.SetActiveAsCheck(xlsxItem.GetMaxCount == -1);
		iconName.text = LanguageMrg.GetText(xlsxItem.Name);
		iconCoin.text = $"<sprite name=Qian>{xlsxItem.Price}";
	}

	public void Enter()
	{
		UiManager.OpenUi<ShopGetWindows>().Init(_xlsxItem);
	}

	public void OnClickGet()
	{
		int getMaxCount = _xlsxItem.GetMaxCount;
		int property = SingletonAsMono<GameDataMrg>.Instance.GetProperty(_xlsxItem.Key, "GetCount", 0L);
		ItemType result;
		if (getMaxCount != -1 && property > getMaxCount)
		{
			UiManager.ShowTips(LanguageMrg.GetText("A1328"));
		}
		else if (SingletonAsMono<GameDataMrg>.Instance.GetProperty("Money", "Property", 0L) >= _xlsxItem.Price && Enum.TryParse<ItemType>(_xlsxItem.Key, out result))
		{
			SingletonAsMono<GameDataMrg>.Instance.AddProperty("Money", -_xlsxItem.Price, "Property");
			SingletonAsMono<GameDataMrg>.Instance.AddProperty(_xlsxItem.Key, 1L, "GetCount");
			new PropertyData
			{
				PropertyType = PropertyType.Item,
				itemType = result,
				PropertyValue = 1
			}.AddTypeValueAsShowTips(isCheck: false);
			UiManager.GetUi<ShopWindows>().Init();
		}
		else
		{
			UiManager.ShowTips(LanguageMrg.GetText("A1230"));
		}
	}

	public void OnClickBuySum()
	{
		UiManager.OpenUi<InputWindows>().Init(LanguageMrg.GetText("A6371"), delegate(string s)
		{
			if (long.TryParse(s, out var result))
			{
				if (SingletonAsMono<GameDataMrg>.Instance.GetProperty("Money", "Property", 0L) >= result * _xlsxItem.Price && Enum.TryParse<ItemType>(_xlsxItem.Key, out var result2))
				{
					SingletonAsMono<GameDataMrg>.Instance.AddProperty("Money", -_xlsxItem.Price * result, "Property");
					SingletonAsMono<GameDataMrg>.Instance.AddProperty(_xlsxItem.Key, result, "GetCount");
					new PropertyData
					{
						PropertyType = PropertyType.Item,
						itemType = result2,
						PropertyValue = (int)result
					}.AddTypeValueAsShowTips(isCheck: false);
					UiManager.GetUi<ShopWindows>().Init();
				}
				else
				{
					UiManager.ShowTips(LanguageMrg.GetText("A6373"));
				}
			}
			else
			{
				UiManager.ShowTips(LanguageMrg.GetText("A6372"));
			}
		}, isCanClose: true, "100");
	}
}
