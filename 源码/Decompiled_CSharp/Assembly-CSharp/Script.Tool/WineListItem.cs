using FrameWork;
using Script.Mrg;
using TMPro;
using UnityEngine;
using Xlsx;

namespace Script.Tool;

public class WineListItem : MonoBehaviour
{
	public Xlsx_WineList_Key xlsxWineListKey;

	public RectTransform point;

	public ShowTipsPos showTipsPos;

	private TipsShow _tipsShow;

	private void OnEnable()
	{
		Xlsx_WineList xlsx_WineList = Xlsx_WineList_Query.XlsxDataAsOneKey.ByKeyGetValue(xlsxWineListKey.ToString());
		GetComponent<TMP_Text>().text = LanguageMrg.GetText(xlsx_WineList.WineListName);
		base.transform.GetChild(0).GetComponent<TMP_Text>().text = $"<sprite name=Qian>{Mathf.Abs(xlsx_WineList.QAdd)}";
	}

	public void OnClick()
	{
		Sure();
	}

	private void Sure()
	{
		Xlsx_WineList xlsx_WineList = Xlsx_WineList_Query.XlsxDataAsOneKey.ByKeyGetValue(xlsxWineListKey.ToString());
		if (SingletonAsMono<GameDataMrg>.Instance.GetProperty("Execution", "Property", 0L) < xlsx_WineList.XDL)
		{
			UiManager.ShowTips(LanguageMrg.GetText("A419"));
		}
		else if (SingletonAsMono<GameDataMrg>.Instance.Coin >= Mathf.Abs(xlsx_WineList.QAdd))
		{
			SingletonAsMono<GameDataMrg>.Instance.xlsxWineListKey = xlsxWineListKey;
			UiManager.GetUi<WineListWindows>().CloseUi();
			new PropertyData
			{
				PropertyType = PropertyType.Property,
				propertyTypeValue = PropertyTypeValue.Money,
				PropertyValue = xlsx_WineList.QAdd
			}.AddTypeValueAsShowTips(isCheck: false);
			if (xlsx_WineList.HGAdd != 0)
			{
				new PropertyData
				{
					PropertyType = PropertyType.Property,
					propertyTypeValue = PropertyTypeValue.LBNFavorability,
					PropertyValue = xlsx_WineList.HGAdd
				}.AddTypeValueAsShowTips(isCheck: false);
			}
		}
		else
		{
			UiManager.ShowTips(LanguageMrg.GetText("A1184"));
		}
	}

	private void OnDisable()
	{
		if (_tipsShow != null)
		{
			UiManager.HideTipsShow(_tipsShow);
			_tipsShow = null;
		}
	}

	public void ShowTips()
	{
		if ((bool)point)
		{
			Xlsx_WineList xlsx_WineList = Xlsx_WineList_Query.XlsxDataAsOneKey.ByKeyGetValue(xlsxWineListKey.ToString());
			_tipsShow = UiManager.ShowTipsShow(LanguageMrg.GetText(xlsx_WineList.Info), point.transform.position, showTipsPos);
		}
	}

	public void HideTips()
	{
		if (_tipsShow != null)
		{
			UiManager.HideTipsShow(_tipsShow);
			_tipsShow = null;
		}
	}
}
