using FrameWork;
using Script.Mrg;
using Script.Tool;
using TMPro;
using UnityEngine;
using Xlsx;

namespace Script.UiTool;

public class TipsTool : MonoBehaviour
{
	public GameObject btn;

	public TMP_Text tipsText;

	public TMP_Text propertyText;

	private Map3DTriggerr _map3DTriggerr;

	public void Init(Map3DTriggerr map3DTriggerr)
	{
		_map3DTriggerr = map3DTriggerr;
		tipsText.text = LanguageMrg.GetText(map3DTriggerr.eventData.ShowText);
		btn.SetActiveAsCheck(active: true);
		if (map3DTriggerr.videoGraph.IsHasAutoEventSAsNotCheckEq(out var autoGroup, isNot360: false))
		{
			for (int i = 0; i < autoGroup.Length; i++)
			{
				string xlsxEventKey = autoGroup[i].xlsxEventKey;
				Xlsx_Event xlsx_Event = Xlsx_Event_Query.XlsxDataAsOneKey.ByKeyGetValue(xlsxEventKey);
				if (xlsx_Event != null && !string.IsNullOrEmpty(xlsx_Event.ShowText))
				{
					tipsText.text = LanguageMrg.GetText(xlsx_Event.ShowText);
					propertyText.text = autoGroup[i].GetPropertyDataStrAsVideoGroup();
					propertyText.SetActive(active: true);
					break;
				}
				propertyText.SetActive(active: false);
			}
		}
		else if (map3DTriggerr.videoGraph != null)
		{
			propertyText.text = map3DTriggerr.videoGraph.GetPropertyDataStrAsVideoGroup();
			propertyText.SetActive(active: true);
		}
		else
		{
			propertyText.SetActive(active: false);
		}
	}

	public void ShowTips(string srt)
	{
		tipsText.text = srt;
		btn.SetActiveAsCheck(active: false);
		propertyText.SetActive(active: false);
	}

	public void Click()
	{
		if (_map3DTriggerr != null)
		{
			SingletonAsMono<GameDataMrg>.Instance.MapLoc = _map3DTriggerr.transform.position;
			_map3DTriggerr.Trigger();
		}
	}
}
